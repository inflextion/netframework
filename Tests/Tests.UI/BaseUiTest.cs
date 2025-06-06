using Allure.Net.Commons;
using atf.Core.Enums;
using atf.UI.PlaywrightSetup;
using Microsoft.Playwright;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;
using Serilog;
using Xunit.Sdk;

namespace atf.Tests.Tests.UI
{
    public abstract class BaseUiTest : IAsyncLifetime, IDisposable
{
    protected IPage? Page { get; private set; } = default!;
    private IBrowserContext Context => Page?.Context;
    /* same as 
     * private IBrowserContext? Context
        {
            get { return Page?.Context; }
        }
     * 
     */
    private IBrowser Browser => Context?.Browser;
    protected ITestOutputHelper OutputHelper { get; private set; }
    protected ILogger Logger { get; private set; }

    public BaseUiTest(ITestOutputHelper output)
    {
        OutputHelper = output;
        
        // Initialize logger for this test instance, using app config and TestOutput sink
        Logger = new LoggerConfiguration()
            //.ReadFrom.Configuration(atf.Core.Config.ConfigManager.Configuration)
            
            .WriteTo.TestOutput(output, outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] [{TestName}] [{Browser}] ({TestContext}) ({Application}) {Message:lj}{NewLine}{Exception}")
            .Enrich.WithProperty("Application", "AutomationFramework")
            .Enrich.WithProperty("TestContext", "XUnit")
            .CreateLogger();
    }

    /// <summary>
    /// Configures test logging for the current test.
    /// Call at the start of each test method for correct test name context.
    /// </summary>
    // You may remove this method if all logging is handled by the per-instance logger.
    //protected virtual void ConfigureTestLogging([CallerMemberName] string testName = null)
    //{
    //    CustomLogger.ConfigureForTest(OutputHelper, testName ?? "UnknownTest");
    //}

    protected async Task TakeScreenshot(string name, bool fullPage = true)
    {
        try
        {
            var screenshot = await Page.ScreenshotAsync(new PageScreenshotOptions
            {
                FullPage = fullPage
            });

            AllureApi.AddAttachment(name, "image/png", screenshot);
        }
        catch (Exception ex)
        {
            Logger.Error("Screenshot failed: {Error}", ex.Message); // Use the instance logger
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
            Logger.Error("Element screenshot failed: {Error}", ex.Message); // Use the instance logger
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
        // Close existing page/context if any
        if (Context is not null)
            await Context.CloseAsync();
        else if (Browser is not null)
            await Browser.CloseAsync();

        // Launch new page with specified browser
        Page = await PlaywrightLauncher.LaunchAsync(browserType);
        
        // Update logger context with browser info
        Logger = Logger.ForContext("Browser", browserType.ToString());
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

        Page = null;
    }

    /// <summary>
    /// Synchronous teardown (called after DisposeAsync).
    /// </summary>
    public void Dispose()
    {
        // If you ever acquire other sync-only resources, clean them up here.
        // No need to reset logger since it's now per-instance and GC will clean up.
    }
}
}