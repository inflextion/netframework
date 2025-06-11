# Playwright .NET Selectors Cheatsheet

## 🎯 Basic Selector Types

### CSS Selectors (Most Common)
```csharp
// By ID
await Page.ClickAsync("#submit-button");

// By Class
await Page.ClickAsync(".btn-primary");

// By Tag
await Page.ClickAsync("button");

// By Attribute
await Page.ClickAsync("[data-testid='login-btn']");
await Page.ClickAsync("input[type='email']");
await Page.ClickAsync("a[href='/home']");
```

### Text-Based Selectors
```csharp
// Exact text match
await Page.ClickAsync("text=Login");
await Page.ClickAsync("'Login'"); // Alternative syntax

// Partial text match
await Page.ClickAsync("text=Log"); // Matches "Login", "Logout", etc.

// Case-insensitive text
await Page.ClickAsync("text=/login/i");

// Text with special characters (use quotes)
await Page.ClickAsync("text='Sign Up Now!'");
```

## 🎨 CSS Selector Combinations

### Hierarchy Selectors
```csharp
// Direct child
await Page.ClickAsync("div > button");

// Descendant (any level)
await Page.ClickAsync("form button");

// Adjacent sibling
await Page.ClickAsync("label + input");

// General sibling
await Page.ClickAsync("h2 ~ p");
```

### Attribute Selectors
```csharp
// Exact match
await Page.ClickAsync("[type='submit']");

// Contains word
await Page.ClickAsync("[class~='active']");

// Starts with
await Page.ClickAsync("[href^='https']");

// Ends with
await Page.ClickAsync("[src$='.png']");

// Contains substring
await Page.ClickAsync("[title*='click']");
```

### Pseudo Selectors
```csharp
// First/Last child
await Page.ClickAsync("li:first-child");
await Page.ClickAsync("li:last-child");

// Nth child
await Page.ClickAsync("tr:nth-child(2)"); // Second row
await Page.ClickAsync("tr:nth-child(odd)"); // Odd rows
await Page.ClickAsync("tr:nth-child(even)"); // Even rows

// Not selector
await Page.ClickAsync("button:not(.disabled)");

// Empty/has content
await Page.ClickAsync("div:empty");
await Page.ClickAsync("div:not(:empty)");
```

## 🔍 Playwright-Specific Selectors

### Has-Text Selector
```csharp
// Element containing specific text
await Page.ClickAsync("button:has-text('Submit')");
await Page.ClickAsync("div:has-text('Error')");

// Case insensitive
await Page.ClickAsync("button:has-text(/submit/i)");

// Partial match
await Page.ClickAsync("span:has-text('Total:')");
```

### Has Selector (Contains Element)
```csharp
// Div that contains a button
await Page.ClickAsync("div:has(button)");

// Form that has required input
await Page.ClickAsync("form:has(input[required])");

// Table row with specific cell content
await Page.ClickAsync("tr:has(td:text('Active'))");
```

### Role-Based Selectors
```csharp
// By ARIA role
await Page.ClickAsync("role=button");
await Page.ClickAsync("role=textbox");
await Page.ClickAsync("role=link");

// Role with name
await Page.ClickAsync("role=button[name='Submit']");
await Page.ClickAsync("role=textbox[name='Email']");
```

## 🎭 Advanced Locator Methods

### Locator Chaining
```csharp
// Chain multiple conditions
var locator = Page.Locator("form")
    .Locator("input[type='email']")
    .First();

// Filter by text
var button = Page.Locator("button")
    .Filter(new() { HasText = "Submit" });

// Filter by child element
var row = Page.Locator("tr")
    .Filter(new() { Has = Page.Locator("td:text('Active')") });
```

### Multiple Elements
```csharp
// Get first matching element
await Page.Locator("button").First().ClickAsync();

// Get last matching element
await Page.Locator("button").Last().ClickAsync();

// Get nth element (0-based)
await Page.Locator("button").Nth(1).ClickAsync(); // Second button

// Count elements
var count = await Page.Locator("button").CountAsync();

// Get all elements
var elements = await Page.Locator("button").AllAsync();
```

## 📋 Common Patterns & Examples

### Forms
```csharp
// Input fields
await Page.FillAsync("input[name='username']", "john");
await Page.FillAsync("role=textbox[name='Password']", "secret");

// Dropdowns
await Page.SelectOptionAsync("select[name='country']", "US");

// Checkboxes/Radio buttons
await Page.CheckAsync("input[type='checkbox']");
await Page.ClickAsync("input[type='radio'][value='yes']");

// Submit
await Page.ClickAsync("button[type='submit']");
await Page.ClickAsync("role=button[name='Submit']");
```

### Navigation
```csharp
// Links
await Page.ClickAsync("a[href='/dashboard']");
await Page.ClickAsync("text=Dashboard");
await Page.ClickAsync("role=link[name='Home']");

// Buttons
await Page.ClickAsync(".nav-button");
await Page.ClickAsync("button:has-text('Menu')");
```

### Tables
```csharp
// Specific cell
await Page.ClickAsync("table tr:nth-child(2) td:nth-child(3)");

// Row with specific content
await Page.ClickAsync("tr:has(td:text('John Doe'))");

// Header
await Page.ClickAsync("th:has-text('Name')");

// All rows
var rows = Page.Locator("tbody tr");
var rowCount = await rows.CountAsync();
```

### Lists
```csharp
// List items
await Page.ClickAsync("ul.menu li:first-child");
await Page.ClickAsync("li:has-text('Settings')");

// Shopping cart example
await Page.ClickAsync(".cart-item:has(span:text('Laptop')) .delete-button");
await Page.ClickAsync(".cart-item:nth-child(1) .quantity-button:has-text('+')");
```

## ⚡ Best Practices

### 1. Selector Priority (Most to Least Reliable)
```csharp
// 1. Test IDs (Best)
await Page.ClickAsync("[data-testid='submit-btn']");

// 2. Semantic roles
await Page.ClickAsync("role=button[name='Submit']");

// 3. Stable attributes
await Page.ClickAsync("[aria-label='Close dialog']");

// 4. Text content (if stable)
await Page.ClickAsync("text=Submit");

// 5. CSS classes (least reliable)
await Page.ClickAsync(".btn-primary");
```

### 2. Robust Selectors
```csharp
// ✅ Good - Specific and stable
await Page.ClickAsync("form[name='login'] button[type='submit']");

// ❌ Bad - Too generic
await Page.ClickAsync("button");

// ✅ Good - Combined approach
await Page.ClickAsync("button[data-testid='save']:has-text('Save')");
```

### 3. Waiting Strategies
```csharp
// Wait for element to be visible
await Page.WaitForSelectorAsync("button[type='submit']");

// Wait and click
await Page.ClickAsync("button", new() { 
    Timeout = 10000 // 10 seconds 
});

// Wait for multiple conditions
await Page.Locator("button").WaitForAsync(new() {
    State = WaitForSelectorState.Visible,
    Timeout = 5000
});
```

## 🚨 Common Pitfalls to Avoid

### 1. Fragile Selectors
```csharp
// ❌ Avoid - Breaks with styling changes
await Page.ClickAsync(".btn.btn-lg.btn-primary.mt-3");

// ✅ Better
await Page.ClickAsync("[data-testid='submit-button']");
```

### 2. Position-Dependent Selectors
```csharp
// ❌ Avoid - Breaks when order changes
await Page.ClickAsync("button:nth-child(3)");

// ✅ Better
await Page.ClickAsync("button:has-text('Delete')");
```

### 3. Missing Waits
```csharp
// ❌ Might fail on slow loading
await Page.ClickAsync("button");

// ✅ Better
await Page.WaitForSelectorAsync("button");
await Page.ClickAsync("button");
```

## 🎯 Quick Reference

| Need | Selector Example |
|------|-----------------|
| Click button with text | `text=Submit` or `button:has-text('Submit')` |
| Fill input by name | `input[name='email']` |
| Select dropdown option | `select[name='country']` → `SelectOptionAsync()` |
| First item in list | `ul li:first-child` |
| Table cell in row 2, col 3 | `tr:nth-child(2) td:nth-child(3)` |
| Element with test ID | `[data-testid='my-element']` |
| Button by role | `role=button[name='Submit']` |
| Element containing text | `:has-text('Error')` |
| Element with child | `:has(button)` |
| Multiple matches | `.Nth(0)`, `.First()`, `.Last()` |

## 🔧 Pro Tips

1. **Use browser dev tools** to test CSS selectors in console: `$$('your-selector')`
2. **Playwright Inspector**: `await Page.PauseAsync()` to debug selectors
3. **Test selectors** in browser console before using in code
4. **Combine approaches**: Use multiple selector strategies for robustness
5. **Keep it simple**: Shorter selectors are often more maintainable