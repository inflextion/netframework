using Xunit.Abstractions;
using atf.Core.Config;
using atf.Core.Models;
using atf.UI.Pages;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;
using Allure.Xunit.Attributes;
using Allure.Net.Commons;
using atf.Data.DatabaseContext;
using Serilog;
using atf.Tests.Helpers;
using atf.Core.Enums;


namespace atf.Tests.Tests.UI
{
    [AllureSuite("BaseTests")]
    [AllureFeature("WebElements")]
    /// <summary>
    /// Smoke test to verify that PlaywrightLauncher launches the configured BaseUrl and a basic textinput validation.
    /// </summary>
    public class BaseUrlLaunchTest : BaseUiTest
    {

        private readonly PlaywrightSettings _settings;
        private readonly ITestOutputHelper _output;
        public BaseUrlLaunchTest(ITestOutputHelper output) : base(output) // <-- Pass to base class!
        {
            _settings = ConfigManager.GetSection<PlaywrightSettings>("Playwright");
            _output = output;
        }

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
            // Use inherited TestLogger with additional context
            var caseLogger = TestLogger.Logger.ForContext("Browser", browserType)
                                             .ForContext("TestMethod", nameof(Should_Launch_BaseUrl));
            
            await RunTestWithScreenshotOnFailure(async () =>
            {
                // Arrange
                // Launch browser with specific type for this test
                await LaunchBrowserAsync(browserType);

                AllureHelper.WriteAllureEnvironmentProperties();
                caseLogger.Information("Test started!");

                var connectionString = ConfigManager.Get<string>("ConnectionStrings:DefaultConnection");
                if (!string.IsNullOrEmpty(connectionString))
                {
                    DbContextFactory.SetConnectionString(connectionString);
                }
                var webElements = new WebElementsPage(Page, _settings, caseLogger);

                // Act
                await webElements.GoToAsync("/webelements");
                caseLogger.Information("Navigating to {Url}", Page.Url);
                await TakeScreenshotAsync($"After navigating to : {Page.Url}");
                await webElements.EnterTextInputAsync("my first text");
                await TakeScreenshotAsync("After Input");

                // Assert - using ILocator from page object
                await Expect(webElements.TextInputOutput).ToContainTextAsync("my first text");
                //TODO: Implement the assertion in the POM 
                //await webElements.AssertTextOutputContains("my first text"); // <-- Move assertion to page object
                webElements.AssertUrlContains(_settings.BaseUrl);
            }, $"failure-{browserType}-{nameof(Should_Launch_BaseUrl)}");
        }
    }
}
