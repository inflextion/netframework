using Xunit.Abstractions;
using atf.Core.Config;
using atf.Core.Models;
using atf.UI.Pages;
using Allure.Xunit.Attributes;
using atf.Core.Enums;
using atf.Tests.Base.UI;
using Serilog;
using atf.Tests.Helpers;
using atf.UI.PlaywrightSetup;

namespace atf.Tests.Tests.UI
{
    [AllureSuite("Advanced Page Tests")]
    [AllureFeature("AdvancedWebElements")]
    public class AdvancedPageTests : BaseUiTest
    {
        private readonly ITestOutputHelper _output;

        public AdvancedPageTests(ITestOutputHelper output)
            : base(output, ConfigManager.GetSection<PlaywrightSettings>("Playwright"))
        {
            _output = output;
        }

        /// <summary>
        /// Sets up the test environment by launching a Playwright browser instance.
        /// </summary>
        /// <remarks>
        /// This method is overridden from the base class to initialize the browser with specific settings.
        /// It uses the Webkit browser, enables video recording and tracing, and names the test based on the class name.
        /// </remarks>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected override async Task OnSetupAsync()
        {
            var testName = GetType().Name;
            Page = await PlaywrightLauncher.LaunchAsync(
                BrowserList.Webkit,
                recordVideo: true,
                enableTracing: true,
                testName: testName
            );
        }

        [Fact(DisplayName = "Shadow DOM Interaction")]
        [Trait("Category", "Smoke")]
        [Trait("Priority", "High")]
        [AllureTag("smoke", "AdvancedWebElements")]
        public async Task AdvancedWebElementsTest()
        {
            // Use inherited TestLogger with additional context
            var caseLogger = TestLogger
                .ForTestMethod(
                    nameof(
                        AdvancedWebElementsTest)) // Creates file: Logs/YourTestClass/AdvancedWebElementsTest_20250624_143022.log
                .ForBrowser(PlaywrightSettings.BrowserType.ToString());
            try
            {
                // Arrange


                AllureHelper.WriteAllureEnvironmentProperties(caseLogger);

                caseLogger.Information("Test started!");

                var advancedWebElements = new AdvancedWebElementsPage(Page, PlaywrightSettings, caseLogger);

                // Act
                await advancedWebElements.GoToAsync("/advanced");
                caseLogger.Information("Navigating to {Url}", Page.Url);
                await TakeScreenshotAsync($"After navigating to : {Page.Url}");
                await advancedWebElements.EnterTextInputAsync("Shadow DOM text");
                await TakeScreenshotAsync("After Input");

                // Assert
                await advancedWebElements.AssertTextOutputAsync("Shadow DOM text");
                advancedWebElements.AssertUrlContains(PlaywrightSettings.BaseUrl);
            }
            catch (Exception ex)
            {
                caseLogger.Error(ex, "❌ Test failed with exception");
                caseLogger.EndTestMethod(nameof(AdvancedWebElementsTest), passed: false);
                throw; // Re-throw to fail the test
            }
        }
    }
}
