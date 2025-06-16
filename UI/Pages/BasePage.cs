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

        /// <summary>
        /// Navigates to the specified relative URL using the base URL from settings.
        /// </summary>
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

        /// <summary>
        /// Waits for the page to finish loading (network idle and DOM content loaded).
        /// </summary>
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

        /// <summary>
        /// Fills the specified selector with the provided text.
        /// </summary>
        protected async Task FillAsync(string selector, string text)
        {
            Logger.Debug("Filling '{Selector}' with '{Text}'", selector, text);
            try
            {
                await Page.Locator(selector).FillAsync(text);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to fill selector '{Selector}' with '{Text}'", selector, text);
                throw;
            }
        }

        /// <summary>
        /// Clicks the element specified by the selector.
        /// </summary>
        protected async Task ClickAsync(string selector)
        {
            Logger.Debug("Clicking '{Selector}'", selector);
            try
            {
                await Page.Locator(selector).ClickAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to click selector '{Selector}'", selector);
                throw;
            }
        }
        
        /// <summary>
        /// Asserts that the text content of the selector contains the expected text.
        /// </summary>
        public async Task AssertOutputContains(string selector, string expectedText)
        {
            try
            {
                var actualText = await Page.Locator(selector).TextContentAsync();
                Xunit.Assert.Contains(expectedText, actualText);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "AssertOutputContains failed. Expected: '{Expected}', Actual: '{Actual}'",
                    expectedText, Page.Locator(selector).TextContentAsync().Result);
                throw;
            }
        }

        /// <summary>
        /// Asserts that the current page URL contains the specified segment.
        /// </summary>
        public void AssertUrlContains(string segment)
        {
            try
            {
                Xunit.Assert.Contains(segment, Page.Url);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "AssertUrlContains failed. Expected segment: '{Segment}', Actual URL: '{Url}'",
                    segment, Page.Url);
                throw;
            }
        }

        /// <summary>
        /// Evaluates JavaScript in the context of the page and returns the result.
        /// </summary>
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

        /// <summary>
        /// Waits for the specified selector to become visible on the page.
        /// </summary>
        protected async Task WaitForVisibleAsync(string selector, int timeoutMs = 5000)
        {
            try
            {
                await Page.Locator(selector)
                    .WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = timeoutMs });
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Wait for selector '{Selector}' to be visible failed.", selector);
                throw;
            }
        }

        /// <summary>
        /// Gets the text content of the specified selector.
        /// </summary>
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

        /// <summary>
        /// Checks (selects) the checkbox or radio button specified by the selector.
        /// </summary>
        protected async Task CheckAsync(string selector)
        {
            try
            {
                await Page.Locator(selector).CheckAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to check selector '{Selector}'", selector);
                throw;
            }
        }

        /// <summary>
        /// Unchecks (deselects) the checkbox specified by the selector.
        /// </summary>
        protected async Task UncheckAsync(string selector)
        {
            try
            {
                await Page.Locator(selector).UncheckAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to uncheck selector '{Selector}'", selector);
                throw;
            }
        }

        /// <summary>
        /// Selects a single option in a select element by value.
        /// </summary>
        protected async Task SelectOptionAsync(string selector, string value)
        {
            try
            {
                await Page.Locator(selector).SelectOptionAsync(value);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to select option '{Value}' in selector '{Selector}'", value, selector);
                throw;
            }
        }
        
        /// <summary>
        /// Selects multiple options in a select element by their values.
        /// </summary>
        /// <param name="selector">The selector for the select element.</param>
        /// <param name="values">The values to select.</param>
        protected async Task SelectOptionsAsync(string selector, params string[] values)
        {
            try
            {
                await Page.Locator(selector).SelectOptionAsync(values);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to select options '{Values}' in selector '{Selector}'", string.Join(", ", values), selector);
                throw;
            }
        }

        /// <summary>
        /// Gets the inner text of the specified selector.
        /// </summary>
        protected async Task<string> InnerTextAsync(string selector)
        {
            try
            {
                return await Page.Locator(selector).InnerTextAsync() ?? string.Empty;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to get inner text from selector '{Selector}'", selector);
                throw;
            }
        }
    }
}
