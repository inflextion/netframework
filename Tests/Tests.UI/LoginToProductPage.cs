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

    [Fact]
    public async Task LoginToProduct()
    {
        var loginPage = new LoginPage(Page, _settings, TestLogger.Logger);
        var productPage = new UserPage(Page, _settings, TestLogger.Logger);

        await loginPage.Login("user", "user");
        await Expect(Page.Locator("h1")).ToHaveTextAsync("Welcome, user");

        var productList = await productPage.ReturnProducts();
        
        
        Assert.Contains("Acer Predator Helios 300", productList.Keys);
        


    }
}