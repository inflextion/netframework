using Microsoft.Playwright;
using Serilog;
using Allure.Net.Commons;
using atf.Core.Models;

namespace atf.UI.Pages
{
    public abstract class BasePage
    {
        protected IPage Page { get; }
        protected PlaywrightSettings Settings { get; }
        protected ILogger Logger { get; }

        protected BasePage(IPage page, PlaywrightSettings settings, ILogger logger)
        {
            Page = page;
            Settings = settings;
            Logger = logger;
        }

        public async Task GoToAsync(string relativeUrl)
        {
            var url = $"{Settings.BaseUrl.TrimEnd('/')}/{relativeUrl.TrimStart('/')}";
            Logger.Information("Navigating to {Url}", url);

            try
            {
                await Page.GotoAsync(url);
                await WaitForPageLoadAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to navigate to {Url}", url);
                throw; // Optionally rethrow, or handle as needed
            }
        }

        public async Task WaitForPageLoadAsync(int timeoutMs = 30_000)
        {
            try
            {
                await Page.WaitForLoadStateAsync(LoadState.NetworkIdle, new() { Timeout = timeoutMs });
                await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded, new() { Timeout = timeoutMs });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error waiting for page load.");
                throw;
            }
        }

        protected async Task FillAsync(string selector, string text)
        {
            Logger.Debug("Filling '{Selector}' with '{Text}'", selector, text);
            try
            {
                await Page.FillAsync(selector, text);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to fill selector '{Selector}' with '{Text}'", selector, text);
                throw;
            }
        }

        protected async Task ClickAsync(string selector)
        {
            Logger.Debug("Clicking '{Selector}'", selector);
            try
            {
                await Page.ClickAsync(selector);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to click selector '{Selector}'", selector);
                throw;
            }
        }

        public void AssertUrlContains(string segment)
        {
            try
            {
                Xunit.Assert.Contains(segment, Page.Url);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "AssertUrlContains failed. Expected segment: '{Segment}', Actual URL: '{Url}'", segment, Page.Url);
                throw;
            }
        }

        public async Task<string> TakeScreenshotAsync(string name, bool fullPage = true)
        {
            var path = Path.Combine("Screenshots", $"{name}-{DateTime.UtcNow:yyyyMMdd-HHmmss}.png");
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

                Logger.Information("Saved screenshot: {Path}", path);
                return path;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to take or save screenshot to {Path}", path);
                throw;
            }
        }

        protected async Task<T> EvalAsync<T>(string script, params object[] args)
        {
            try
            {
                return await Page.EvaluateAsync<T>(script, args);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "JavaScript evaluation failed: {Script}", script);
                throw;
            }
        }

        protected async Task WaitForVisibleAsync(string selector, int timeoutMs = 5000)
        {
            try
            {
                await Page.Locator(selector).WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = timeoutMs });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Wait for selector '{Selector}' to be visible failed.", selector);
                throw;
            }
        }

        protected async Task<string> GetTextAsync(string selector)
        {
            try
            {
                return await Page.Locator(selector).TextContentAsync() ?? string.Empty;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to get text content from selector '{Selector}'", selector);
                throw;
            }
        }
    }
}
