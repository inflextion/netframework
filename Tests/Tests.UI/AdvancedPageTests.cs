using Xunit.Abstractions;
using atf.Core.Config;
using atf.Core.Models;
using atf.UI.Pages;
using Allure.Xunit.Attributes;
using Serilog;
using atf.Tests.Helpers;

namespace atf.Tests.Tests.UI
{
    [AllureSuite("Advanced Page Tests")]
    [AllureFeature("AdvancedWebElements")]
    public class AdvancedPageTests : BaseUiTest
    {
        PlaywrightSettings _settings;
        private readonly ITestOutputHelper _output;

        public AdvancedPageTests(ITestOutputHelper output) : base(output)
        {
            _settings = ConfigManager.GetSection<PlaywrightSettings>("Playwright");
            _output = output;
        }

        [Fact(DisplayName = "Shadow DOM Interaction")]
        [Trait("Category", "Smoke")]
        [Trait("Priority", "High")]
        [AllureTag("smoke", "AdvancedWebElements")]
        public async Task AdvancedWebElementsTest()
        {
            // Arrange
            // Launch browser from appsettings for this test
            AllureHelper.WriteAllureEnvironmentProperties();

            // Use inherited TestLogger with additional context
            var caseLogger = TestLogger.Logger.ForContext("Browser", _settings.BrowserType)
                                             .ForContext("TestMethod", nameof(AdvancedWebElementsTest));
            caseLogger.Information("Test started!");


            var advancedWebElements = new AdvancedWebElementsPage(Page, _settings, caseLogger);

            // Act
            await advancedWebElements.GoToAsync("/advanced");
            caseLogger.Information("Navigating to {Url}", Page.Url);
            await TakeScreenshotAsync($"After navigating to : {Page.Url}");
            await advancedWebElements.EnterTextInputAsync("Shadow DOM text");
            await TakeScreenshotAsync("After Input");

            // Assert
            await advancedWebElements.AssertTextOutputAsync("Shadow DOM text");
            advancedWebElements.AssertUrlContains(_settings.BaseUrl);
        }
    }
}
