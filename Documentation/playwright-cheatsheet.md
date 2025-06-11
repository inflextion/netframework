# Playwright Cheatsheet

A comprehensive reference guide for Playwright automation in C# with practical examples for test automation.

## 🎯 Basic Navigation

```csharp
// Navigate to URL
await Page.GotoAsync("https://example.com");

// Go back/forward
await Page.GoBackAsync();
await Page.GoForwardAsync();

// Reload page
await Page.ReloadAsync();

// Wait for page load
await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
await Page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
```

## 🔍 Element Selectors

### CSS Selectors
```csharp
// By ID
await Page.ClickAsync("#submit-button");

// By Class
await Page.ClickAsync(".btn-primary");

// By Attribute
await Page.ClickAsync("[data-testid='login-btn']");
await Page.ClickAsync("[name='username']");
await Page.ClickAsync("[type='submit']");

// By Tag
await Page.ClickAsync("button");

// Descendant
await Page.ClickAsync(".form-container input[type='text']");

// Child selector
await Page.ClickAsync(".navbar > .nav-item");

// Pseudo-selectors
await Page.ClickAsync("button:first-child");
await Page.ClickAsync("li:nth-child(3)");
await Page.ClickAsync("input:not([disabled])");
```

### XPath Selectors
```csharp
// By text content
await Page.ClickAsync("//button[text()='Submit']");
await Page.ClickAsync("//span[contains(text(), 'Welcome')]");

// By attribute
await Page.ClickAsync("//input[@placeholder='Enter email']");
await Page.ClickAsync("//div[@class='modal']//button[@type='submit']");

// By position
await Page.ClickAsync("//ul/li[1]");  // First item
await Page.ClickAsync("//ul/li[last()]");  // Last item

// Complex XPath
await Page.ClickAsync("//table//tr[td[contains(text(), 'John')]]/td[3]/button");
```

### Playwright-Specific Selectors
```csharp
// By text (case-insensitive)
await Page.ClickAsync("text=Submit");
await Page.ClickAsync("text=/submit/i");  // Regex, case-insensitive

// By role
await Page.ClickAsync("role=button[name='Submit']");
await Page.ClickAsync("role=link[name='Home']");
await Page.ClickAsync("role=textbox[name='Username']");

// By test ID
await Page.ClickAsync("data-testid=submit-btn");

// By label
await Page.FillAsync("label=Username", "john@example.com");
await Page.FillAsync("label=/email/i", "test@test.com");

// By placeholder
await Page.FillAsync("placeholder=Enter your email", "user@example.com");
```

## 🎯 Locator Methods

### Creating Locators
```csharp
// Create locator (doesn't perform action immediately)
var submitButton = Page.Locator("#submit-btn");
var firstRow = Page.Locator("table tr").First();
var lastItem = Page.Locator(".list-item").Last();

// Chaining locators
var modal = Page.Locator(".modal");
var closeButton = modal.Locator("button[aria-label='Close']");

// Filter locators
var visibleButtons = Page.Locator("button").Locator("visible=true");
var enabledInputs = Page.Locator("input").Locator("enabled=true");
```

### Locator Filters
```csharp
// Filter by text
var linkWithText = Page.Locator("a").Filter(new() { HasText = "Click here" });

// Filter by nested element
var cardWithButton = Page.Locator(".card").Filter(new() { 
    Has = Page.Locator("button[type='submit']") 
});

// Filter by not having text
var cardsWithoutText = Page.Locator(".card").Filter(new() { 
    HasNotText = "Sold out" 
});
```

## 🖱️ Mouse Actions

```csharp
// Basic clicks
await Page.ClickAsync("#button");
await Page.DblClickAsync("#element");
await Page.ClickAsync("#element", new() { Button = MouseButton.Right });

// Click with modifiers
await Page.ClickAsync("#link", new() { Modifiers = new[] { KeyboardModifier.Control } });

// Click at position
await Page.ClickAsync("#element", new() { Position = new Position { X = 10, Y = 20 } });

// Hover
await Page.HoverAsync("#menu-item");

// Drag and drop
await Page.DragAndDropAsync("#source", "#target");

// Mouse wheel
await Page.Mouse.WheelAsync(0, 200);  // Scroll down
```

## ⌨️ Keyboard Actions

```csharp
// Type text
await Page.FillAsync("#input", "Hello World");
await Page.TypeAsync("#input", "Hello World", new() { Delay = 100 });

// Clear and type
await Page.FillAsync("#input", "");  // Clear
await Page.FillAsync("#input", "New text");

// Keyboard shortcuts
await Page.Keyboard.PressAsync("Control+A");  // Select all
await Page.Keyboard.PressAsync("Control+C");  // Copy
await Page.Keyboard.PressAsync("Control+V");  // Paste
await Page.Keyboard.PressAsync("Escape");     // Escape key

// Special keys
await Page.PressAsync("#input", "Tab");
await Page.PressAsync("#input", "Enter");
await Page.PressAsync("#input", "Backspace");
```

## 📋 Form Interactions

```csharp
// Input fields
await Page.FillAsync("#username", "john@example.com");
await Page.FillAsync("#password", "secretpassword");

// Checkboxes and radio buttons
await Page.CheckAsync("#terms-checkbox");
await Page.UncheckAsync("#newsletter");
await Page.SetCheckedAsync("#subscribe", true);

// Select dropdowns
await Page.SelectOptionAsync("#country", "US");
await Page.SelectOptionAsync("#city", new[] { "NYC", "LA" });  // Multiple
await Page.SelectOptionAsync("#state", new SelectOptionValue() { Label = "California" });

// File uploads
await Page.SetInputFilesAsync("#file-upload", "/path/to/file.pdf");
await Page.SetInputFilesAsync("#multiple-files", new[] { "file1.txt", "file2.txt" });
```

## ⏱️ Waiting Strategies

```csharp
// Wait for element
await Page.WaitForSelectorAsync("#loading-spinner", new() { State = WaitForSelectorState.Hidden });
await Page.WaitForSelectorAsync(".success-message", new() { State = WaitForSelectorState.Visible });

// Wait for function
await Page.WaitForFunctionAsync("() => document.readyState === 'complete'");

// Wait for response
await Page.WaitForResponseAsync("**/api/data");
await Page.WaitForResponseAsync(response => response.Url.Contains("/api/") && response.Status == 200);

// Wait for load state
await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

// Locator waits (built-in)
await Page.Locator("#submit").ClickAsync();  // Auto-waits for element to be clickable

// Custom timeout
await Page.ClickAsync("#slow-button", new() { Timeout = 10000 });  // 10 seconds
```

## 🔍 Assertions with Expect

```csharp
// Element visibility
await Expect(Page.Locator("#success-message")).ToBeVisibleAsync();
await Expect(Page.Locator("#loading")).ToBeHiddenAsync();

// Text content
await Expect(Page.Locator("h1")).ToHaveTextAsync("Welcome");
await Expect(Page.Locator(".error")).ToContainTextAsync("Invalid");

// Attributes
await Expect(Page.Locator("#submit")).ToHaveAttributeAsync("disabled", "");
await Expect(Page.Locator("#username")).ToHaveValueAsync("john@example.com");

// Count
await Expect(Page.Locator(".list-item")).ToHaveCountAsync(5);

// Page assertions
await Expect(Page).ToHaveTitleAsync("Dashboard");
await Expect(Page).ToHaveURLAsync("**/dashboard");

// Custom timeout for assertions
await Expect(Page.Locator("#slow-element")).ToBeVisibleAsync(new() { Timeout = 10000 });
```

## 📱 Multi-Element Operations

```csharp
// Get all elements
var allLinks = await Page.Locator("a").AllAsync();
foreach (var link in allLinks)
{
    var href = await link.GetAttributeAsync("href");
    Console.WriteLine($"Link: {href}");
}

// Count elements
var itemCount = await Page.Locator(".list-item").CountAsync();

// Get element by index
await Page.Locator(".tab").Nth(2).ClickAsync();  // Click 3rd tab (0-indexed)

// First and last
await Page.Locator("button").First().ClickAsync();
await Page.Locator("li").Last().ClickAsync();

// Filter and click
await Page.Locator("button").Filter(new() { HasText = "Delete" }).ClickAsync();
```

## 🎭 Multiple Contexts and Pages

```csharp
// Multiple pages in same context
var page1 = await Context.NewPageAsync();
var page2 = await Context.NewPageAsync();

// Switch between pages
await page1.BringToFrontAsync();
await page2.BringToFrontAsync();

// Handle new page (popup)
var newPageTask = Context.WaitForPageAsync();
await Page.ClickAsync("#open-popup");
var newPage = await newPageTask;
```

## 📸 Screenshots and Videos

```csharp
// Full page screenshot
await Page.ScreenshotAsync(new() { Path = "screenshot.png" });

// Element screenshot
await Page.Locator("#chart").ScreenshotAsync(new() { Path = "chart.png" });

// PDF generation
await Page.PdfAsync(new() { Path = "page.pdf", Format = "A4" });

// Video recording (configured in BrowserContext)
var context = await Browser.NewContextAsync(new() {
    RecordVideoDir = "videos/",
    RecordVideoSize = new() { Width = 1280, Height = 720 }
});
```

## 🚫 Error Handling

```csharp
// Try-catch for element operations
try
{
    await Page.ClickAsync("#optional-button", new() { Timeout = 5000 });
}
catch (TimeoutException)
{
    Console.WriteLine("Button not found, continuing...");
}

// Check if element exists
if (await Page.Locator("#error-message").IsVisibleAsync())
{
    var errorText = await Page.Locator("#error-message").TextContentAsync();
    throw new Exception($"Page error: {errorText}");
}

// Conditional actions
var isLoggedIn = await Page.Locator("#logout-btn").IsVisibleAsync();
if (!isLoggedIn)
{
    await Page.ClickAsync("#login-btn");
}
```

## 🎨 Advanced Selectors

### Combining Selectors
```csharp
// CSS + Text
await Page.ClickAsync("button:has-text('Submit')");

// CSS + Visible
await Page.ClickAsync("input:visible");

// Multiple conditions
await Page.ClickAsync("button.primary:has-text('Save'):visible");

// Parent-child relationships
await Page.ClickAsync(".modal >> button[type='submit']");  // >> is deprecated, use nested
await Page.Locator(".modal").Locator("button[type='submit']").ClickAsync();
```

### Dynamic Selectors
```csharp
// Variables in selectors
var userId = "12345";
await Page.ClickAsync($"[data-user-id='{userId}']");

// Parameterized locators
public async Task ClickUserRow(string username)
{
    await Page.ClickAsync($"//tr[td[text()='{username}']]//button[@title='Edit']");
}

// Regex in text selectors
await Page.ClickAsync("text=/save|submit/i");  // Case-insensitive save OR submit
```

## ⚡ Performance Tips

```csharp
// Use locators instead of finding elements repeatedly
var submitButton = Page.Locator("#submit");
await submitButton.ClickAsync();
await submitButton.IsEnabledAsync();

// Parallel operations where possible
var tasks = new[]
{
    Page.FillAsync("#username", "user"),
    Page.FillAsync("#password", "pass"),
    Page.CheckAsync("#remember-me")
};
await Task.WhenAll(tasks);

// Use auto-waiting instead of manual waits
// ❌ Manual wait
await Page.WaitForSelectorAsync("#element");
await Page.ClickAsync("#element");

// ✅ Auto-wait
await Page.ClickAsync("#element");  // Automatically waits for element
```

## 🛠️ Debugging

```csharp
// Slow motion for debugging
var context = await Browser.NewContextAsync(new() { SlowMo = 500 });

// Pause execution
await Page.PauseAsync();  // Opens Playwright Inspector

// Console logging
await Page.EvaluateAsync("console.log('Debug message')");

// Get element info
var element = Page.Locator("#debug-element");
var text = await element.TextContentAsync();
var isVisible = await element.IsVisibleAsync();
Console.WriteLine($"Element text: {text}, Visible: {isVisible}");
```

## 📋 Common Patterns

### Login Helper
```csharp
public async Task LoginAsync(string username, string password)
{
    await Page.FillAsync("#username", username);
    await Page.FillAsync("#password", password);
    await Page.ClickAsync("#login-button");
    await Expect(Page.Locator("#dashboard")).ToBeVisibleAsync();
}
```

### Table Data Extraction
```csharp
public async Task<List<string>> GetTableRowDataAsync()
{
    var rows = Page.Locator("table tbody tr");
    var count = await rows.CountAsync();
    var data = new List<string>();
    
    for (int i = 0; i < count; i++)
    {
        var cellText = await rows.Nth(i).Locator("td").First().TextContentAsync();
        data.Add(cellText);
    }
    
    return data;
}
```

### Modal Handling
```csharp
public async Task HandleModalAsync(string buttonText)
{
    // Wait for modal to appear
    await Expect(Page.Locator(".modal")).ToBeVisibleAsync();
    
    // Click button in modal
    await Page.Locator(".modal").Locator($"button:has-text('{buttonText}')").ClickAsync();
    
    // Wait for modal to disappear
    await Expect(Page.Locator(".modal")).ToBeHiddenAsync();
}
```

This cheatsheet covers the most commonly used Playwright features for web automation testing. Keep it handy for quick reference during test development!