using System.Diagnostics.CodeAnalysis;
using atf.Core.Config;
using atf.Core.Models;
using atf.Tests.Base.UI;
using atf.UI.Pages;
using Xunit.Abstractions;
using static Microsoft.Playwright.Assertions;


namespace atf.Tests.Tests.UI;

public class LoginToProductPage : BaseUiTest
{
    private readonly PlaywrightSettings _settings;
    private readonly ITestOutputHelper _output;
    public LoginToProductPage(ITestOutputHelper output) : base(output) // <-- Pass to base class!
    {
        _settings = ConfigManager.GetSection<PlaywrightSettings>("Playwright");
        _output = output;
    }

    [Fact(DisplayName = "Login to Product Page")] 
    public async Task LoginToProduct()
    {
        var caseLogger = TestLogger
            .ForTestMethod(nameof(LoginToProduct)) // Automatically adds start separator
            .ForBrowser(PlaywrightSettings.BrowserType.ToString());

        try 
        {
            var loginPage = new LoginPage(Page, PlaywrightSettings, caseLogger);
            var productPage = new UserPage(Page, PlaywrightSettings, caseLogger);

            caseLogger.Information("🔐 Attempting login with user credentials");
        
            await loginPage.Login("user", "user");
            caseLogger.ForStep("Login").Information("✅ Login successful - user authenticated");
        
            await Expect(Page.Locator("h1")).ToHaveTextAsync("Welcome, user");
            caseLogger.ForStep("Verification").Information("✅ Welcome message verified");
        
            var productList = await productPage.ReturnProducts();
            caseLogger.ForStep("Product Retrieval").Information("📦 Retrieved {ProductCount} products", productList.Count);
        
            Assert.Contains("Acer Predator Helios 300", productList.Keys);
            caseLogger.Information("✅ Target product found in list");
        
            // Test passed
            caseLogger.EndTestMethod(nameof(LoginToProduct), passed: true);
        }
        catch (Exception ex)
        {
            caseLogger.Error(ex, "❌ Test failed with exception");
            caseLogger.EndTestMethod(nameof(LoginToProduct), passed: false);
            throw; // Re-throw to fail the test
        }
    }
}