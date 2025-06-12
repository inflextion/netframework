using atf.Core.Models;
using Microsoft.Playwright;
using Serilog;
using static Microsoft.Playwright.Assertions;

namespace atf.UI.Pages
{
    public class AdvancedWebElementsPage : BasePage
    {
        // Selectors as constants
        private const string TextInputSelector = ".advanced-text-input-section input[type='text']";
        private const string TextInputOutputSelector = ".advanced-text-input-section .output";
        private const string CounterValueSelector = ".advanced-counter-section .counter-value";
        private const string CounterIncrementSelector = ".advanced-counter-section .increment-btn";
        private const string CounterDecrementSelector = ".advanced-counter-section .decrement-btn";
        private const string CounterResetSelector = ".advanced-counter-section .reset-btn";
        private const string DropdownSelector = ".advanced-dropdown-section select";
        private const string DropdownOutputSelector = ".advanced-dropdown-section .output";
        private const string CheckboxSelector = ".advanced-checkbox-section input[type='checkbox']";
        private const string CheckboxOutputSelector = ".advanced-checkbox-section .output";
        private const string RadioInputSelector = ".advanced-radio-section input[type='radio']";
        private const string RadioOutputSelector = ".advanced-radio-section .output";
        private const string ToggleEnableButtonSelector = ".advanced-toggle-section .enable-btn";
        private const string ToggleOutputSelector = ".advanced-toggle-section .output";

        public AdvancedWebElementsPage(IPage page, PlaywrightSettings settings, ILogger logger) 
            : base(page, settings, logger)
        {
        }

        // 1. Text Input - using BasePage methods
        public async Task EnterTextInputAsync(string text) =>
            await FillAsync(TextInputSelector, text);

        public async Task<string> GetTextInputOutputAsync() =>
            await InnerTextAsync(TextInputOutputSelector);

        public async Task AssertTextOutputAsync(string text) =>
            await AssertOutputContains(TextInputOutputSelector, text);

        // 2. Counter - using BasePage methods
        public async Task<string> GetCounterValueAsync() =>
            await InnerTextAsync(CounterValueSelector);

        public async Task ClickCounterIncrementAsync() =>
            await ClickAsync(CounterIncrementSelector);

        public async Task ClickCounterDecrementAsync() =>
            await ClickAsync(CounterDecrementSelector);

        public async Task ClickCounterResetAsync() =>
            await ClickAsync(CounterResetSelector);

        // 3. Dropdown
        public async Task SelectDropdownAsync(string value) =>
            await SelectOptionAsync(DropdownSelector, value);

        public async Task<string> GetDropdownOutputAsync() =>
            await InnerTextAsync(DropdownOutputSelector);

        // 4. Checkbox Group
        public async Task CheckCheckboxAsync(int index) =>
            await CheckAsync($"{CheckboxSelector}:nth-child({index + 1})");

        public async Task UncheckCheckboxAsync(int index) =>
            await UncheckAsync($"{CheckboxSelector}:nth-child({index + 1})");

        public async Task<string> GetCheckboxOutputAsync() =>
            await InnerTextAsync(CheckboxOutputSelector);

        // 5. Radio Buttons
        public async Task SelectRadioAsync(int index) =>
            await CheckAsync($"{RadioInputSelector}:nth-child({index + 1})");

        public async Task<string> GetRadioOutputAsync() =>
            await InnerTextAsync(RadioOutputSelector);

        // 6. Enable/Disable Toggle
        public async Task ClickToggleEnableAsync() =>
            await ClickAsync(ToggleEnableButtonSelector);

        public async Task<string> GetToggleOutputAsync() =>
            await InnerTextAsync(ToggleOutputSelector);
    }
}