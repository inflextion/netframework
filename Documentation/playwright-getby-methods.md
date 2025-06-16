# Playwright .NET 9 — GetBy Methods for FCKIT Accademy Demo Page

This document provides reusable C# methods to interact with the FCKIT Accademy demo page, using Playwright's modern `GetBy...` queries.
All methods are idiomatic for Playwright .NET 9.

---

## Navigation Links

Interact with top navigation using accessible names.

```csharp
public async Task ClickNavLinkAsync(IPage page, string linkName)
{
    await page.GetByRole(AriaRole.Link, new() { Name = linkName }).ClickAsync();
}

// Usage example:
// await ClickNavLinkAsync(page, "Elements (beginner)");
```

---

## Shadow DOM Input

Fill the input labeled “Shadow DOM Input:”.

```csharp
public async Task EnterTextInShadowDomInputAsync(IPage page, string text)
{
    await page.GetByLabel("Shadow DOM Input:").FillAsync(text);
}

// Usage:
// await EnterTextInShadowDomInputAsync(page, "Hello World");
```

---

## Counter Buttons

Click the “-”, “+”, or “Reset” counter buttons.

```csharp
public async Task ClickCounterButtonAsync(IPage page, string buttonText)
{
    await page.GetByRole(AriaRole.Button, new() { Name = buttonText }).ClickAsync();
}

// Usage:
// await ClickCounterButtonAsync(page, "+");
// await ClickCounterButtonAsync(page, "-");
// await ClickCounterButtonAsync(page, "Reset");
```

---

## Dropdown Select

Select an option from the dropdown by visible text.

```csharp
public async Task SelectDropdownOptionAsync(IPage page, string option)
{
    var dropdown = page.GetByLabel("Dropdown:");
    await dropdown.SelectOptionAsync(new SelectOptionValue { Label = option });
}

// Usage:
// await SelectDropdownOptionAsync(page, "Option 2");
```

---

## Checkbox Group

Check any of the “Option A”, “Option B”, or “Option C” checkboxes.

```csharp
public async Task SetCheckboxAsync(IPage page, string label)
{
    await page.GetByLabel(label).CheckAsync();
}

// Usage:
// await SetCheckboxAsync(page, "Option A");
// await SetCheckboxAsync(page, "Option B");
```

---

## Radio Buttons

Select a radio option by its visible label.

```csharp
public async Task SelectRadioOptionAsync(IPage page, string label)
{
    await page.GetByLabel(label).CheckAsync();
}

// Usage:
// await SelectRadioOptionAsync(page, "Option 2");
```

---

## Enable/Disable Toggle Button

Toggle the “Enable” button to change the status.

```csharp
public async Task ToggleEnableDisableAsync(IPage page)
{
    await page.GetByRole(AriaRole.Button, new() { Name = "Enable" }).ClickAsync();
    // To toggle back, look for the "Disable" button, if present.
}
```

---

## Reading Output Text

Get the value from output fields (e.g., Counter, Dropdown, Checkbox, Radio, Toggle status).

```csharp
public async Task<string> GetOutputTextAsync(IPage page, string labelText)
{
    // Looks for the <p> sibling after a given label
    var label = page.GetByText(labelText);
    var output = label.Locator("xpath=following-sibling::p[1]");
    return await output.InnerTextAsync();
}

// Usage for counter output:
// string counterValue = await GetOutputTextAsync(page, "Counter:");
```

---

## Summary Table

| UI Element       | Selector            | Method Name                      |
| ---------------- | ------------------- | -------------------------------- |
| Navigation link  | `GetByRole`         | `ClickNavLinkAsync`              |
| Shadow DOM Input | `GetByLabel`        | `EnterTextInShadowDomInputAsync` |
| Counter buttons  | `GetByRole`         | `ClickCounterButtonAsync`        |
| Dropdown         | `GetByLabel`        | `SelectDropdownOptionAsync`      |
| Checkbox         | `GetByLabel`        | `SetCheckboxAsync`               |
| Radio button     | `GetByLabel`        | `SelectRadioOptionAsync`         |
| Toggle button    | `GetByRole`         | `ToggleEnableDisableAsync`       |
| Output reader    | `GetByText + Xpath` | `GetOutputTextAsync`             |

---

## Notes

* Prefer `GetByLabel` for form elements with visible labels.
* Prefer `GetByRole` for buttons, links, and navigation.
* All methods assume you have initialized your `IPage` object and navigated to the target page.
* For more complex queries, consider adding further locators or assertions as needed.

---

**Feel free to adapt or extend these methods for your test suite!**
