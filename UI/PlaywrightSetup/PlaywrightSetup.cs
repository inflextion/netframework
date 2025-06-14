﻿using atf.Core.Config;
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
        private static readonly PlaywrightSettings Settings;

        static PlaywrightLauncher()
        {
            try
            {
                Settings = ConfigManager.GetSection<PlaywrightSettings>("Playwright");
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
                var browserType = browserTypeOverride ?? Settings.BrowserType;

                IBrowser browser = browserType switch
                {
                    BrowserList.Firefox => await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions { Headless = Settings.Headless }),
                    BrowserList.Webkit => await playwright.Webkit.LaunchAsync(new BrowserTypeLaunchOptions { Headless = Settings.Headless }),
                    BrowserList.Chrome => await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Channel = "chrome", Headless = Settings.Headless }),
                    BrowserList.Edge => await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Channel = "msedge", Headless = Settings.Headless }),
                    _ => await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = Settings.Headless }),
                };

                var context = await browser.NewContextAsync(new BrowserNewContextOptions
                {
                    BaseURL = Settings.BaseUrl,
                    ViewportSize = new ViewportSize { Width = Settings.ViewportWidth, Height = Settings.ViewportHeight }
                });

                var page = await context.NewPageAsync();
                page.SetDefaultTimeout(Settings.DefaultTimeoutMs);
                return page;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to launch Playwright page.", ex);
            }
        }
}
}