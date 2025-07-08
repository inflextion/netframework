using atf.Core.Enums;
using atf.Core.Logging;
using atf.Core.Models;
using atf.Tests.Helpers;
using atf.UI.PlaywrightSetup;
using Microsoft.Playwright;
using Xunit.Abstractions;

namespace atf.Tests.Base.UI
{
    public abstract class BaseUiTest : IAsyncLifetime, IDisposable
    {
        protected IPage Page { get; set; } = null!;
        protected IBrowserContext Context => Page?.Context;
        protected IBrowser Browser => Context?.Browser;
        protected ITestOutputHelper OutputHelper { get; private set; }
        protected TestLogger TestLogger { get; private set; }
        protected PlaywrightSettings PlaywrightSettings { get; }
        
        protected BaseUiTest(ITestOutputHelper output, PlaywrightSettings? playwrightSettings = null)
        {
            OutputHelper = output;
            PlaywrightSettings = playwrightSettings ?? atf.UI.PlaywrightSetup.PlaywrightLauncher.Settings;
            // Single logger that always writes to file
            TestLogger = new TestLogger(output, GetType().Name, "TBD");
    
            TestLogger.Information("Test class initialized: {ClassName}", GetType().Name);
        }


        protected async Task TakeScreenshotAsync(string name, bool fullPage = true)
        {
            try
            {
                var path = await ScreenshotHelper.TakeScreenshotAsync(Page, name, fullPage);
                TestLogger.Information("Saved screenshot: {Path}", path);
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

        /// <summary>
        /// Handles a popup window triggered by an action and performs a specified operation on the popup.
        /// </summary>
        /// <typeparam name="T">The type of the result returned by the popup action.</typeparam>
        /// <param name="triggerAction">The action that triggers the popup.</param>
        /// <param name="popupAction">The operation to perform on the popup window.</param>
        /// <returns>A task representing the asynchronous operation, with a result of type <typeparamref name="T"/>.</returns>
        protected Task<T> HandlePopupAsync<T>(Func<Task> triggerAction, Func<IPage, Task<T>> popupAction)
            => PlaywrightHelpers.HandlePopupAsync(Context, TestLogger, triggerAction, popupAction);

        /// <summary>
        /// Handles a popup window triggered by an action and performs a specified operation on the popup.
        /// </summary>
        /// <param name="triggerAction">The action that triggers the popup.</param>
        /// <param name="popupAction">The operation to perform on the popup window.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected Task HandlePopupAsync(Func<Task> triggerAction, Func<IPage, Task> popupAction)
            => PlaywrightHelpers.HandlePopupAsync(Context, TestLogger, triggerAction, popupAction);

        /// <summary>
        /// Handles a JavaScript dialog triggered by an action, such as an alert, confirm, or prompt.
        /// </summary>
        /// <param name="triggerAction">The action that triggers the dialog.</param>
        /// <param name="accept">Indicates whether to accept the dialog. Defaults to true.</param>
        /// <param name="promptText">The text to provide for a prompt dialog, if applicable. Defaults to null.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected Task HandleDialogAsync(Func<Task> triggerAction, bool accept = true, string promptText = null)
            => PlaywrightHelpers.HandleDialogAsync(Page, TestLogger, triggerAction, accept, promptText);

        /// <summary>
        /// Executes a test action and automatically takes a screenshot if the test fails.
        /// </summary>
        /// <param name="testAction">The test logic to execute</param>
        /// <param name="screenshotName">Name for the failure screenshot (default: "test-failure")</param>
        protected async Task RunTestWithScreenshotOnFailure(Func<Task> testAction,
            string screenshotName = "test-failure")
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
            await BeforeBrowserLaunchAsync();
            await OnSetupAsync();
            await AfterBrowserLaunchAsync();
        }

        /// <summary>
        /// Hook for logic before browser launch. Override in derived classes if needed.
        /// </summary>
        protected virtual async Task BeforeBrowserLaunchAsync() { }

        /// <summary>
        /// Hook for logic after browser launch. Override in derived classes if needed.
        /// </summary>
        protected virtual async Task AfterBrowserLaunchAsync() { }

        protected virtual async Task OnSetupAsync()
        {
            OnSetup();
            var testName = GetType().Name;
            Page = await PlaywrightLauncher.LaunchAsync(
                testName: testName);
        }

        protected virtual void OnSetup()
        {
            
        }

        /// <summary>
        /// Launches a new page with a specific browser type for cross-browser testing.
        /// Call this method in tests that need a specific browser.
        /// </summary>
        /// <param name="browserType">The browser type to launch</param>
        /// <param name="recordVideo">Whether to record video for this browser session</param>
        /// <param name="enableTracing">Whether to enable tracing (overrides config setting)</param>
        

        /// <summary>
        /// Called once after all tests in the class have run.
        /// </summary>
        public virtual async Task DisposeAsync()
        {
            try
            {
                // Stop tracing before closing context
                if (Context != null)
                {
                    var testName = GetType().Name;
                    await PlaywrightLauncher.StopTracingAsync(Context, testName);
                }

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