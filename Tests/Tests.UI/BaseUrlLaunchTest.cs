using Xunit.Abstractions;
using atf.Core.Config;
using atf.Core.Models;
using atf.UI.Pages;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;
using atf.UI.Helpers;
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
       
        public BaseUrlLaunchTest(ITestOutputHelper output) : base(output) // <-- Pass to base class!
        {
            _settings = ConfigManager.GetSection<PlaywrightSettings>("Playwright");
        }

        [Theory]
        [InlineData(BrowserList.Chromium)]
        [InlineData(BrowserList.Firefox)]
        [InlineData(BrowserList.Webkit)]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureOwner("QA Team")]
        [Trait("Category", "Smoke")]
        [Trait("Priority", "High")]
        [AllureTag("smoke", "login")]
        public async Task Should_Launch_BaseUrl(BrowserList browserType)
        {
            var testName = $"{nameof(Should_Launch_BaseUrl)}_{browserType}_{DateTime.UtcNow:yyyyMMdd_HHmmss}";
            await RunTestWithScreenshotOnFailure(async () =>
            {
                // Arrange
                // Launch browser with specific type for this test
                await LaunchBrowserAsync(browserType);

                AllureHelper.WriteAllureEnvironmentProperties();

                var logFile = $"Logs/{testName}.log";
                var caseLogger = new LoggerConfiguration()
                    .WriteTo.File(logFile,
                        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] [{TestName}] [{Browser}] ({TestContext}) ({Application}) {Message:lj}{NewLine}{Exception}")
                    .Enrich.WithProperty("TestName", testName)
                    .Enrich.WithProperty("Browser", browserType)
                    .CreateLogger();
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
                await TakeScreenshot($"After navigating to : {Page.Url}");
                await webElements.FillInTextInputBox("my first text");
                await webElements.TakeScreenshotAsync("After Input"); // <-- Move screenshot logic to page object

                // Assert
                await Expect(Page.Locator(WebElementsSelectors.TextOutput)).ToContainTextAsync("my first text");
                //TODO: Implement the assertion in the POM 
                //await Expect( webElements.AssertTextOutputContains("my first text"); // <-- Move assertion to page object
                webElements.AssertUrlContains(_settings.BaseUrl);
            }, $"failure-{browserType}-{testName}");
        }
    }
}
