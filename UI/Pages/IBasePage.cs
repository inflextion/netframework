using Microsoft.Playwright;

namespace atf.UI.Pages;

public interface IBasePage
{
    Task GoToAsync(string relativeUrl);
    Task WaitForPageLoadAsync(int timeoutMs = 30_000);
    Task AssertOutputContains(string selector, string expectedText);
    void AssertUrlContains(string segment);
    
    // String-based methods
    Task FillAsync(string locator, string text);
    Task ClickAsync(string locator);
    Task WaitForVisibleAsync(string selector, int timeoutMs = 5000);
    Task<string> GetTextAsync(string selector);
    Task CheckAsync(string selector);
    Task UncheckAsync(string selector);
    Task SelectOptionAsync(string selector, string value);
    Task SelectOptionsAsync(string selector, params string[] values);
    Task<string> InnerTextAsync(string selector);
    Task<TResult> EvalAsync<TResult>(string script, params object[] args);
}