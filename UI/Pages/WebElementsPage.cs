using atf.Core.Models;
using Microsoft.Playwright;
using Serilog;

namespace atf.UI.Pages
{
    public class WebElementsPage : BasePage
    {
        // Selectors as constants
        public const string TextInputSelector = "#text-input";
        public const string TextInputOutputSelector = ".web-element:has(#text-input) .web-element-output";
        public const string CounterValueSelector = ".web-element-counter-value";
        public const string CounterIncrementButtonSelector = ".web-element-counter button:has-text(\"+\")";
        public const string CounterDecrementButtonSelector = ".web-element-counter button:has-text(\"-\")";
        public const string CounterResetButtonSelector = ".web-element-button-reset";
        public const string CounterErrorSelector = ".web-element-error";
        public const string DropdownSelector = "#dropdown";
        public const string DropdownOutputSelector = ".web-element:has(#dropdown) .web-element-output";
        public const string CheckboxSelector = ".web-element-checkbox";
        public const string CheckboxOutputSelector = ".web-element-checkbox-group ~ .web-element-output";
        public const string RadioOutputSelector = ".web-element-radio-group ~ .web-element-output";
        public const string ToggleEnableButtonSelector = ".web-element-toggle-button.enabled";
        public const string ToggleOutputSelector = ".web-element-toggle-group ~ .web-element-output";

        public WebElementsPage(IPage page, PlaywrightSettings settings, ILogger logger) : base(page, settings, logger)
        {
        }

        // 1. Text Input - using BasePage methods
        public async Task EnterTextInputAsync(string text) =>
            await FillAsync(TextInputSelector, text);

        public async Task<string> GetTextInputOutputAsync() =>
            await InnerTextAsync(TextInputOutputSelector);

        // 2. Counter - using BasePage methods
        public async Task ClickCounterIncrementAsync() =>
            await ClickAsync(CounterIncrementButtonSelector);

        public async Task ClickCounterDecrementAsync() =>
            await ClickAsync(CounterDecrementButtonSelector);

        public async Task ClickCounterResetAsync() =>
            await ClickAsync(CounterResetButtonSelector);

        public async Task<string> GetCounterValueAsync() =>
            await GetTextAsync(CounterValueSelector);

        public async Task<string> GetCounterErrorAsync() =>
            await GetTextAsync(CounterErrorSelector);

        // 3. Dropdown
        public async Task SelectDropdownAsync(string value) =>
            await SelectOptionAsync(DropdownSelector, value);

        public async Task<string> GetDropdownOutputAsync() =>
            await GetTextAsync(DropdownOutputSelector);

        // 4. Checkbox Options
        public async Task CheckCheckboxAsync(int index) =>
            await CheckAsync($"{CheckboxSelector}:nth-child({index + 1})");

        public async Task UncheckCheckboxAsync(int index) =>
            await UncheckAsync($"{CheckboxSelector}:nth-child({index + 1})");

        public async Task<string> GetCheckboxOutputAsync() =>
            await GetTextAsync(CheckboxOutputSelector);

        // 5. Radio Buttons
        public async Task SelectRadioAsync(string value) =>
            await CheckAsync($".web-element-radio-input[value='{value}']");

        public async Task<string> GetRadioOutputAsync() =>
            await GetTextAsync(RadioOutputSelector);

        // 6. Enable/Disable Toggle
        public async Task ClickToggleEnableAsync() =>
            await ClickAsync(ToggleEnableButtonSelector);

        public async Task<string> GetToggleOutputAsync() =>
            await GetTextAsync(ToggleOutputSelector);
    }
}