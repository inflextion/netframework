using System.Diagnostics.CodeAnalysis;
using atf.Core.Models;
using atf.Data.Models;
using Microsoft.Playwright;
using Serilog;
using static Microsoft.Playwright.Assertions;

namespace atf.UI.Pages;

public class LoginPage:BasePageILocator
{
    public ILocator UserNameInput => Page.GetByPlaceholder("Username");
    public ILocator PasswordInput => Page.GetByRole(AriaRole.Textbox, new() {Name = "password" });
    public ILocator LoginButton => Page.GetByRole(AriaRole.Button, new() { Name = "Login" });

    public LoginPage(IPage page, PlaywrightSettings settings, ILogger logger) : base(page, settings, logger){}

    public async Task Login(string username, string password)
    {
        await GoToAsync("/form");
        await FillAsync(UserNameInput, username);
        await FillAsync(PasswordInput, password);
        await ClickAsync(LoginButton);
        
        await WaitForPageLoadAsync();

    }

}

