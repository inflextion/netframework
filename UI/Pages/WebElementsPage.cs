using atf.Core.Models;
using Microsoft.Playwright;
using Serilog;

namespace atf.UI.Pages
{
    public class WebElementsPage : BasePage
    {
        // Locators as ILocator properties - initialized in constructor
        public ILocator TextInput { get; }
        public ILocator TextInputOutput { get; }
        public ILocator CounterValue { get; }
        public ILocator CounterIncrementButton { get; }
        public ILocator CounterDecrementButton { get; }
        public ILocator CounterResetButton { get; }
        public ILocator CounterError { get; }
        public ILocator Dropdown { get; }
        public ILocator DropdownOutput { get; }
        public ILocator CheckboxGroup { get; }
        public ILocator Checkbox { get; }
        public ILocator CheckboxOutput { get; }
        public ILocator RadioGroup { get; }
        public ILocator RadioInput { get; }
        public ILocator RadioOutput { get; }
        public ILocator ToggleEnableButton { get; }
        public ILocator ToggleOutput { get; }

        public WebElementsPage(IPage page, PlaywrightSettings settings, ILogger logger) : base(page, settings, logger)
        {
            // Initialize locators using Page.Locator()
            TextInput = Page.Locator("#text-input");
            TextInputOutput = Page.Locator(".web-element:has(#text-input) .web-element-output");
            
            CounterValue = Page.Locator(".web-element-counter-value");
            CounterIncrementButton = Page.Locator(".web-element-counter button:has-text(\"+\")");
            CounterDecrementButton = Page.Locator(".web-element-counter button:has-text(\"-\")");
            CounterResetButton = Page.Locator(".web-element-button-reset");
            CounterError = Page.Locator(".web-element-error");
            
            Dropdown = Page.Locator("#dropdown");
            DropdownOutput = Page.Locator(".web-element:has(#dropdown) .web-element-output");
            
            CheckboxGroup = Page.Locator(".web-element-checkbox-group");
            Checkbox = Page.Locator(".web-element-checkbox");
            CheckboxOutput = Page.Locator(".web-element-checkbox-group ~ .web-element-output");
            
            RadioGroup = Page.Locator(".web-element-radio-group");
            RadioInput = Page.Locator(".web-element-radio-input");
            RadioOutput = Page.Locator(".web-element-radio-group ~ .web-element-output");
            
            ToggleEnableButton = Page.Locator(".web-element-toggle-button.enabled");
            ToggleOutput = Page.Locator(".web-element-toggle-group ~ .web-element-output");
        }

        // 1. Text Input - using ILocator directly
        public async Task EnterTextInputAsync(string text)
        {
            Logger.Debug("Filling text input with '{Text}'", text);
            try
            {
                await TextInput.FillAsync(text);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Failed to fill text input with '{Text}'", text);
                throw;
            }
        }

        public async Task<string> GetTextInputOutputAsync() =>
            await TextInputOutput.InnerTextAsync();

        // 2. Counter - using ILocator methods
        public async Task ClickCounterIncrementAsync() =>
            await CounterIncrementButton.ClickAsync();

        public async Task ClickCounterDecrementAsync() =>
            await CounterDecrementButton.ClickAsync();

        public async Task ClickCounterResetAsync() =>
            await CounterResetButton.ClickAsync();

        public async Task<string> GetCounterValueAsync() =>
            await CounterValue.TextContentAsync() ?? "";

        public async Task<string> GetCounterErrorAsync() =>
            await CounterError.TextContentAsync() ?? "";

        // 3. Dropdown
        public async Task SelectDropdownAsync(string value) =>
            await Dropdown.SelectOptionAsync(value);

        public async Task<string> GetDropdownOutputAsync() =>
            await DropdownOutput.TextContentAsync() ?? "";

        // 4. Checkbox Options
        public async Task CheckCheckboxAsync(int index) =>
            await Checkbox.Nth(index).CheckAsync();

        public async Task UncheckCheckboxAsync(int index) =>
            await Checkbox.Nth(index).UncheckAsync();

        public async Task<string> GetCheckboxOutputAsync() =>
            await CheckboxOutput.TextContentAsync() ?? "";

        // 5. Radio Buttons
        public async Task SelectRadioAsync(string value) =>
            await Page.Locator($".web-element-radio-input[value='{value}']").CheckAsync();

        public async Task<string> GetRadioOutputAsync() =>
            await RadioOutput.TextContentAsync() ?? "";

        // 6. Enable/Disable Toggle
        public async Task ClickToggleEnableAsync() =>
            await ToggleEnableButton.ClickAsync();

        public async Task<string> GetToggleOutputAsync() =>
            await ToggleOutput.TextContentAsync() ?? "";
    }
}