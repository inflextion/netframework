using atf.Core.Enums;
using atf.Core.Models;
using atf.UI.PlaywrightSetup;
using Xunit.Abstractions;

namespace atf.Tests.Base.UI;

public class DynamicBrowser : BaseUiTest
{
    public DynamicBrowser(ITestOutputHelper output, PlaywrightSettings? playwrightSettings = null)
        : base(output, playwrightSettings)
    {
        // This constructor allows for dynamic browser type selection at runtime
        // by passing the desired BrowserList value to the base class.
    }

    public override async Task InitializeAsync()
    {
        // Do not launch a browser here. Let the test method call LaunchBrowserAsync with the desired browser type.
        // This avoids launching a default browser before the test sets the correct one.
    }
    protected async Task LaunchBrowserAsync(BrowserList browserType, bool recordVideo = false,
        bool? enableTracing = null)
    {
        // Properly close existing browser resources
        if (Page?.Context is not null)
            await Page.Context.CloseAsync();

        if (Page?.Context?.Browser is not null)
            await Page.Context.Browser.CloseAsync();

        // Launch new page with specified browser and video recording option
        var testName = $"{GetType().Name}-{browserType}";
        Page = await PlaywrightLauncher.LaunchAsync(browserType, recordVideo, enableTracing, testName);

        // Browser context will be added via ForContext() in individual tests
        TestLogger.Information($"Browser launched: {browserType}, RecordVideo: {recordVideo}", browserType,
            recordVideo);
    }

    
}