using atf.Core.Config;
using atf.Core.Enums;
using atf.Core.Models;
using Microsoft.Playwright;

namespace atf.UI.PlaywrightSetup
{
    /// <summary>
    /// Launches Playwright browser/page for UI tests configured via appsettings.json.
    /// </summary>
    public static class PlaywrightLauncher
    {
        private static readonly PlaywrightSettings _settings;

        static PlaywrightLauncher()
        {
            try
            {
                _settings = ConfigManager.GetSection<PlaywrightSettings>("Playwright");
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load Playwright settings from configuration.", ex);
            }
        }

        /// <summary>
        /// Launches a new Playwright page using settings from config, or a browser type override if provided.
        /// </summary>
        /// <param name="browserTypeOverride">Optionally specify a browser type for this instance (e.g., BrowserList.Chromium).</param>
        /// <returns>A new IPage instance.</returns>
        public static async Task<IPage> LaunchAsync(BrowserList? browserTypeOverride = null)
        {
            try
            {
                var playwright = await Playwright.CreateAsync();

                // Use override if provided, else use config
                var browserType = browserTypeOverride ?? _settings.BrowserType;

                IBrowser browser = browserType switch
                {
                    BrowserList.Firefox => await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = _settings.Headless }),
                    BrowserList.Webkit => await playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions { Headless = _settings.Headless }),
                    BrowserList.Chrome => await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Channel = "chrome", Headless = _settings.Headless }),
                    BrowserList.Edge => await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Channel = "msedge", Headless = _settings.Headless }),
                    _ => await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = _settings.Headless }),
                };

                var context = await browser.NewContextAsync(new BrowserNewContextOptions
                {
                    BaseURL = _settings.BaseUrl,
                    ViewportSize = new ViewportSize { Width = _settings.ViewportWidth, Height = _settings.ViewportHeight }
                });

                var page = await context.NewPageAsync();
                page.SetDefaultTimeout(_settings.DefaultTimeoutMs);
                return page;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to launch Playwright page.", ex);
            }
        }
}
}