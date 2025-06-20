using atf.Core.Enums;
using atf.Core.Logging;
using atf.Tests.Helpers;
using atf.UI.PlaywrightSetup;
using Microsoft.Playwright;
using Xunit.Abstractions;

namespace atf.Tests.Base.UI
{
    public abstract class BaseUiTest : IAsyncLifetime, IDisposable
{
    protected IPage Page { get; private set; } = default!;
    protected IBrowserContext Context => Page?.Context;
    private IBrowser Browser => Context?.Browser;
    protected ITestOutputHelper OutputHelper { get; private set; }
    protected TestLogger TestLogger { get; private set; }

    protected BaseUiTest(ITestOutputHelper output)
    {
        OutputHelper = output;
        TestLogger = new TestLogger(output, GetType().Name, writeToFile: true, browserType: "TBD");
    }


    protected async Task<string> TakeScreenshotAsync(string name, bool fullPage = true)
    {
        try
        {
            var path = await ScreenshotHelper.TakeScreenshotAsync(Page, name, fullPage);
            TestLogger.Information("Saved screenshot: {Path}", path);
            return path;
        }
        catch (Exception ex)
        {
            TestLogger.Error(ex, "Failed to take or save screenshot");
            throw;
        }
    }

    protected async Task TakeElementScreenshot(ILocator element, string name)
    {
        try
        {
            await ScreenshotHelper.TakeElementScreenshotAsync(element, name);
        }
        catch (Exception ex)
        {
            TestLogger.Error("Element screenshot failed: {Error}", ex.Message);
        }
    }

    // Use PlaywrightHelpers for popup and dialog handling
    protected Task<T> HandlePopupAsync<T>(Func<Task> triggerAction, Func<IPage, Task<T>> popupAction)
        => PlaywrightHelpers.HandlePopupAsync(Context, TestLogger, triggerAction, popupAction);

    protected Task HandlePopupAsync(Func<Task> triggerAction, Func<IPage, Task> popupAction)
        => PlaywrightHelpers.HandlePopupAsync(Context, TestLogger, triggerAction, popupAction);

    protected Task HandleDialogAsync(Func<Task> triggerAction, bool accept = true, string promptText = null)
        => PlaywrightHelpers.HandleDialogAsync(Page, TestLogger, triggerAction, accept, promptText);

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
    /// <param name="recordVideo">Whether to record video for this browser session</param>
    protected async Task LaunchBrowserAsync(BrowserList browserType, bool recordVideo = false)
    {
        // Properly close existing browser resources
        if (Page?.Context is not null)
            await Page.Context.CloseAsync();
        
        if (Page?.Context?.Browser is not null)
            await Page.Context.Browser.CloseAsync();

        // Launch new page with specified browser and video recording option
        Page = await PlaywrightLauncher.LaunchAsync(browserType, recordVideo);
        
        // Browser context will be added via ForContext() in individual tests
        TestLogger.Information($"Browser launched: {browserType}, RecordVideo: {recordVideo}", browserType, recordVideo);
    }

    /// <summary>
    /// Called once after all tests in the class have run.
    /// </summary>
    public virtual async Task DisposeAsync()
    {
        try
        {
            // Close in reverse order of creation: Page → Context → Browser
            if (Page != null)
            {
                await Page.CloseAsync();
                Page = null;
            }
            
            if (Context != null)
            {
                await Context.CloseAsync();
            }
            
            if (Browser != null)
            {
                await Browser.CloseAsync();
            }
        }
        finally
        {
            // Finally block always executes, even if exceptions occur above
            TestLogger?.Dispose();
        }
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