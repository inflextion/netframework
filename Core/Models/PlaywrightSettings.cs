using atf.Core.Enums;

namespace atf.Core.Models
{
    /// <summary>
    /// Maps to the "Playwright" section in appsettings.json.
    /// </summary>
    public class PlaywrightSettings
    {
        public BrowserList BrowserType { get; set; } = BrowserList.Chromium;
        public bool Headless { get; set; } = true;
        public string BaseUrl { get; set; } = string.Empty;
        public string BaseApiHost { get; set; } = string.Empty;
        public int DefaultTimeoutMs { get; set; } = 30000;
        public int ViewportWidth { get; set; } = 1280;
        public int ViewportHeight { get; set; } = 800;
        public int SlowMoMs { get; set; } = 3000; 
    }
}