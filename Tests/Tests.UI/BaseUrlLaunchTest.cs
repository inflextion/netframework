using Xunit.Abstractions;
using atf.Core.Config;
using atf.Core.Models;
using atf.UI.Pages;
using Allure.Xunit.Attributes;
using Allure.Net.Commons;
using atf.Tests.Helpers;
using atf.Core.Enums;
using atf.Tests.Base.UI;


namespace atf.Tests.Tests.UI
{
    [AllureSuite("BaseTests")]
    [AllureFeature("WebElements")]
    public class BaseUrlLaunchTest : DynamicBrowser
    {
        private readonly PlaywrightSettings _settings;
        private readonly ITestOutputHelper _output;

        public BaseUrlLaunchTest(ITestOutputHelper output) : base(output,
            ConfigManager.GetSection<PlaywrightSettings>("Playwright")) // <-- Pass to base class!
        {
            _settings = ConfigManager.GetSection<PlaywrightSettings>("Playwright");
            _output = output;
        }

        /// <summary>
        /// Smoke test to verify that PlaywrightLauncher launches the configured BaseUrl and a basic textinput validation.
        /// </summary>
        [Theory]
        [InlineData(BrowserList.Edge)]
        [InlineData(BrowserList.Firefox)]
        [InlineData(BrowserList.Webkit)]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureOwner("QA Team")]
        [Trait("Category", "Smoke")]
        [Trait("Priority", "High")]
        [AllureTag("smoke", "login")]
        public async Task Should_Launch_BaseUrl(BrowserList browserType)
        {
            // Launch browser with specific type for this test
            await LaunchBrowserAsync(browserType);

            // Use TestLogger's ForTestMethod and ForContext to get a TestLogger with context
            var caseLogger = TestLogger
                .ForTestMethod(nameof(Should_Launch_BaseUrl)) // Creates file: Logs/YourTestClass/AdvancedWebElementsTest_20250624_143022.log
                .ForBrowser(browserType.ToString());
            
            AllureHelper.WriteAllureEnvironmentProperties(caseLogger);
            caseLogger.Information("Test started!");
            var webElements = new WebElementsPage(Page, _settings, caseLogger);

            // Act
            await webElements.GoToAsync("/webelements");
            caseLogger.Information("Navigating to {Url}", Page.Url);
            await TakeScreenshotAsync($"After navigating to : {Page.Url}");
            await webElements.EnterTextInputAsync("my first text");
            await TakeScreenshotAsync("After Input");

            // Assert - using BasePage assertion method
            webElements.AssertUrlContains(_settings.BaseUrl);
            // }, $"failure-{browserType}-{nameof(Should_Launch_BaseUrl)}");
        }
    }
}