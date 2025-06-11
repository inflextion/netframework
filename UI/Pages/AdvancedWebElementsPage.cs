using atf.Core.Models;
using Microsoft.Playwright;
using Serilog;
using static Microsoft.Playwright.Assertions;

namespace atf.UI.Pages
{
    public class AdvancedWebElementsPage : BasePage
    {
        // ILocator properties for advanced web elements
        public ILocator TextInputSection { get; }
        public ILocator TextInput { get; }
        public ILocator TextInputOutput { get; }
        public ILocator CounterSection { get; }
        public ILocator CounterValue { get; }
        public ILocator CounterIncrement { get; }
        public ILocator CounterDecrement { get; }
        public ILocator CounterReset { get; }
        public ILocator DropdownSection { get; }
        public ILocator Dropdown { get; }
        public ILocator DropdownOutput { get; }
        public ILocator CheckboxSection { get; }
        public ILocator Checkbox { get; }
        public ILocator CheckboxOutput { get; }
        public ILocator RadioSection { get; }
        public ILocator RadioInput { get; }
        public ILocator RadioOutput { get; }
        public ILocator ToggleSection { get; }
        public ILocator ToggleEnableButton { get; }
        public ILocator ToggleOutput { get; }

        public AdvancedWebElementsPage(IPage page, PlaywrightSettings settings, ILogger logger) 
            : base(page, settings, logger)
        {
            // Initialize locators using Page.Locator()
            TextInputSection = Page.Locator(".advanced-text-input-section");
            TextInput = TextInputSection.Locator("input[type='text']");
            TextInputOutput = TextInputSection.Locator(".output");

            CounterSection = Page.Locator(".advanced-counter-section");
            CounterValue = CounterSection.Locator(".counter-value");
            CounterIncrement = CounterSection.Locator(".increment-btn");
            CounterDecrement = CounterSection.Locator(".decrement-btn");
            CounterReset = CounterSection.Locator(".reset-btn");

            DropdownSection = Page.Locator(".advanced-dropdown-section");
            Dropdown = DropdownSection.Locator("select");
            DropdownOutput = DropdownSection.Locator(".output");

            CheckboxSection = Page.Locator(".advanced-checkbox-section");
            Checkbox = CheckboxSection.Locator("input[type='checkbox']");
            CheckboxOutput = CheckboxSection.Locator(".output");

            RadioSection = Page.Locator(".advanced-radio-section");
            RadioInput = RadioSection.Locator("input[type='radio']");
            RadioOutput = RadioSection.Locator(".output");

            ToggleSection = Page.Locator(".advanced-toggle-section");
            ToggleEnableButton = ToggleSection.Locator(".enable-btn");
            ToggleOutput = ToggleSection.Locator(".output");
        }

        // 1. Text Input - using ILocator directly
        public async Task EnterTextInputAsync(string text) =>
            await TextInput.FillAsync(text);

        public async Task<string> GetTextInputOutputAsync() =>
            await TextInputOutput.InnerTextAsync();

        public async Task AssertTextOutputAsync(string text) =>
            await Expect(TextInputOutput).ToContainTextAsync(text);

        // 2. Counter - using ILocator methods
        public async Task<string> GetCounterValueAsync() =>
            await CounterValue.InnerTextAsync();

        public async Task ClickCounterIncrementAsync() =>
            await CounterIncrement.ClickAsync();

        public async Task ClickCounterDecrementAsync() =>
            await CounterDecrement.ClickAsync();

        public async Task ClickCounterResetAsync() =>
            await CounterReset.ClickAsync();

        // 3. Dropdown
        public async Task SelectDropdownAsync(string value) =>
            await Dropdown.SelectOptionAsync(value);

        public async Task<string> GetDropdownOutputAsync() =>
            await DropdownOutput.InnerTextAsync();

        // 4. Checkbox Group
        public async Task CheckCheckboxAsync(int index) =>
            await Checkbox.Nth(index).CheckAsync();

        public async Task UncheckCheckboxAsync(int index) =>
            await Checkbox.Nth(index).UncheckAsync();

        public async Task<string> GetCheckboxOutputAsync() =>
            await CheckboxOutput.InnerTextAsync();

        // 5. Radio Buttons
        public async Task SelectRadioAsync(int index) =>
            await RadioInput.Nth(index).CheckAsync();

        public async Task<string> GetRadioOutputAsync() =>
            await RadioOutput.InnerTextAsync();

        // 6. Enable/Disable Toggle
        public async Task ClickToggleEnableAsync() =>
            await ToggleEnableButton.ClickAsync();

        public async Task<string> GetToggleOutputAsync() =>
            await ToggleOutput.InnerTextAsync();
    }
}