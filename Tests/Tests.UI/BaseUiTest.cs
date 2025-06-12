using Allure.Net.Commons;
using atf.Core.Enums;
using atf.Core.Logging;
using atf.UI.PlaywrightSetup;
using Microsoft.Playwright;
using Serilog;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace atf.Tests.Tests.UI
{
    public abstract class BaseUiTest : IAsyncLifetime, IDisposable
{
    protected IPage Page { get; private set; } = default!;
    protected IBrowserContext Context => Page?.Context;
    private IBrowser Browser => Context?.Browser;
    protected ITestOutputHelper OutputHelper { get; private set; }
    protected TestLogger TestLogger { get; private set; }

    public BaseUiTest(ITestOutputHelper output)
    {
        OutputHelper = output;
        TestLogger = new TestLogger(output, GetType().Name, writeToFile: true, browserType: "TBD");
    }


    protected async Task<string> TakeScreenshotAsync(string name, bool fullPage = true)
    {
        // Sanitize the name to remove invalid filename characters
        var invalidChars = Path.GetInvalidFileNameChars();
        var safeName = string.Join("_", name.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries)).Trim('_');
        var path = Path.Combine("Screenshots", $"{safeName}-{DateTime.UtcNow:yyyyMMdd-HHmmss}.png");
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            var bytes = await Page.ScreenshotAsync(new() { FullPage = fullPage });
            await File.WriteAllBytesAsync(path, bytes);

            AllureApi.AddAttachment(
                name: "Failure_Screenshot",
                type: "image/png",
                content: bytes,
                fileExtension: "png"
            );

            TestLogger.Information("Saved screenshot: {Path}", path);
            return path;
        }
        catch (Exception ex)
        {
            TestLogger.Error(ex, "Failed to take or save screenshot to {Path}", path);
            throw;
        }
    }

    protected async Task TakeElementScreenshot(ILocator element, string name)
    {
        try
        {
            var screenshot = await element.ScreenshotAsync();
            AllureApi.AddAttachment(name, "image/png", screenshot);
        }
        catch (Exception ex)
        {
            TestLogger.Error("Element screenshot failed: {Error}", ex.Message);
        }
    }

    /// <summary>
    /// Handles popup windows by waiting for them and executing actions
    /// </summary>
    protected async Task<T> HandlePopupAsync<T>(Func<Task> triggerAction, Func<IPage, Task<T>> popupAction)
    {
        TestLogger.Information("Setting up popup handler");
        
        // Set up popup handler before triggering action
        var popupTask = Context.WaitForPageAsync();
        
        // Execute the action that triggers the popup
        await triggerAction();
        
        // Wait for popup and execute actions on it
        var popup = await popupTask;
        TestLogger.Information("Popup opened: {Url}", popup.Url);
        
        try
        {
            var result = await popupAction(popup);
            return result;
        }
        finally
        {
            await popup.CloseAsync();
            TestLogger.Information("Popup closed");
        }
    }

    /// <summary>
    /// Handles popup windows without return value
    /// </summary>
    protected async Task HandlePopupAsync(Func<Task> triggerAction, Func<IPage, Task> popupAction)
    {
        await HandlePopupAsync(triggerAction, async popup =>
        {
            await popupAction(popup);
            return true; // Dummy return value
        });
    }

    /// <summary>
    /// Handles JavaScript alerts, confirms, and prompts
    /// </summary>
    protected async Task HandleDialogAsync(Func<Task> triggerAction, bool accept = true, string promptText = null)
    {
        TestLogger.Information("Setting up dialog handler - Accept: {Accept}", accept);
        
        Page.Dialog += async (_, dialog) =>
        {
            TestLogger.Information("Dialog appeared: Type={Type}, Message='{Message}'", dialog.Type, dialog.Message);
            
            if (accept)
            {
                if (!string.IsNullOrEmpty(promptText))
                    await dialog.AcceptAsync(promptText);
                else
                    await dialog.AcceptAsync();
            }
            else
            {
                await dialog.DismissAsync();
            }
        };
        
        await triggerAction();
    }

    /// <summary>
    /// Executes a test action and automatically takes a screenshot if the test fails.
    /// </summary>
    /// <param name="testAction">The test logic to execute</param>
    /// <param name="screenshotName">Name for the failure screenshot (default: "test-failure")</param>
    protected async Task RunTestWithScreenshotOnFailure(Func<Task> testAction, string screenshotName = "test-failure")
    {
        try
        {
            await testAction();
        }
        catch (Exception)
        {
            await TakeScreenshotAsync(screenshotName);
            throw; // Re-throw to maintain normal test failure behavior
        }
    }

    /// <summary>
    /// Called once before any test in the class runs.
    /// </summary>
    public virtual async Task InitializeAsync()
    {
        Page = await PlaywrightLauncher.LaunchAsync();
       
    }

    /// <summary>
    /// Launches a new page with a specific browser type for cross-browser testing.
    /// Call this method in tests that need a specific browser.
    /// </summary>
    /// <param name="browserType">The browser type to launch</param>
    protected async Task LaunchBrowserAsync(BrowserList browserType)
    {
        // Properly close existing browser resources
        if (Page?.Context is not null)
            await Page.Context.CloseAsync();
        
        if (Page?.Context?.Browser is not null)
            await Page.Context.Browser.CloseAsync();

        // Launch new page with specified browser
        Page = await PlaywrightLauncher.LaunchAsync(browserType);
        
        // Browser context will be added via ForContext() in individual tests
        TestLogger.Information("Browser launched: {BrowserType}", browserType);
    }

    /// <summary>
    /// Called once after all tests in the class have run.
    /// </summary>
    public virtual async Task DisposeAsync()
    {
        if (Context is not null)
            await Context.CloseAsync();
        else if (Browser is not null)
            await Browser.CloseAsync();

        TestLogger?.Dispose();
        Page = null;
    }

    /// <summary>
    /// Synchronous teardown (called after DisposeAsync).
    /// </summary>
    public void Dispose()
    {
        TestLogger?.Dispose();
    }
}
}