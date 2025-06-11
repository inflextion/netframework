# .NET Playwright Workshop Program

*A 3-session intensive hands-on workshop for programmers transitioning to .NET test automation*

**Total Duration:** 6 hours (3 sessions × 2 hours)  
**Target Audience:** Developers with programming experience (any language)  
**Prerequisites:** Basic programming knowledge, Git familiarity  

---

## 🎯 **Workshop Objectives**

By the end of this workshop, participants will be able to:
- Set up a .NET test automation project from scratch
- Write maintainable UI tests using Playwright
- Implement Page Object Model patterns
- Create API tests with RestSharp
- Build a complete test suite with proper reporting

---

## 📋 **Session Overview**

| Session | Focus | Duration | Key Topics |
|---------|-------|----------|------------|
| **Session 1** | .NET Fundamentals & Playwright Basics | 2 hours | Setup, C# syntax, first tests |
| **Session 2** | Page Object Model & Advanced Playwright | 2 hours | POM, interactions, debugging |
| **Session 3** | API Testing & Framework Integration | 2 hours | RestSharp, data-driven tests, Allure reporting |

---

# 🚀 **Session 1: .NET Fundamentals & Playwright Basics**
*Duration: 2 hours*

## **Part 1: Environment Setup (30 minutes)**

### **Prerequisites Installation**
```bash
# Install .NET 9 SDK
# Download from: https://dotnet.microsoft.com/download

# Verify installation
dotnet --version

# Install IDE (choose one)
# - Visual Studio 2022 Community (Windows/Mac)
# - JetBrains Rider (cross-platform)
# - VS Code with C# extension
```

### **Project Setup**
```bash
# Create new test project
dotnet new xunit -n PlaywrightWorkshop
cd PlaywrightWorkshop

# Add required packages
dotnet add package Microsoft.Playwright
dotnet add package Microsoft.Playwright.XUnit
dotnet add package Microsoft.Extensions.Configuration
dotnet add package Microsoft.Extensions.Configuration.Json

# Install Playwright browsers
dotnet build
pwsh bin/Debug/net9.0/playwright.ps1 install
```

### **Project Structure Setup**
```
PlaywrightWorkshop/
├── Pages/
├── Tests/
├── Models/
├── Helpers/
├── appsettings.json
└── PlaywrightWorkshop.csproj
```

## **Part 2: C# Basics for Test Automation (45 minutes)**

### **C# Syntax Essentials**
```csharp
// Variables and types
string siteName = "Example Site";
int timeout = 30000;
bool isVisible = true;
var autoInferred = "This is a string"; // Type inference

// Collections
var usernames = new List<string> { "admin", "user1", "user2" };
var testData = new Dictionary<string, object>
{
    { "username", "testuser" },
    { "password", "secret123" }
};

// Properties (instead of getters/setters)
public class User
{
    public string Name { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; } = true; // Default value
}

// Async/await pattern
public async Task<string> GetPageTitleAsync()
{
    var title = await Page.TitleAsync();
    return title;
}
```

### **xUnit Test Structure with Base Classes**
```csharp
using Xunit;
using Microsoft.Playwright;
using atf.Tests.Base;
using atf.Core.Config;
using Serilog;

// Base test class (located in Tests/Base/BaseUiTest.cs)
public abstract class BaseUiTest : IAsyncLifetime
{
    protected IPage Page;
    protected IBrowser Browser;
    protected ILogger Logger;
    protected BaseUiTest()
    {
        Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
    }

    public async Task InitializeAsync()
    {
        var playwright = await Playwright.CreateAsync();
        var browserType = ConfigManager.Get<string>("Playwright:BrowserType");
        var headless = ConfigManager.Get<bool>("Playwright:Headless");
        
        Browser = await playwright.Chromium.LaunchAsync(new()
        {
            Headless = headless,
            SlowMo = headless ? 0 : 1000
        });
        
        Page = await Browser.NewPageAsync();
        Logger.Information("Browser initialized: {BrowserType}", browserType);
    }

    public async Task DisposeAsync()
    {
        await Browser?.CloseAsync();
        Logger.Information("Browser disposed");
    }

    protected async Task TakeScreenshotAsync(string name)
    {
        await Page.ScreenshotAsync(new() 
        { 
            Path = $"screenshots/{name}_{DateTime.Now:yyyyMMdd_HHmmss}.png" 
        });
    }
}

// Actual test class
public class BasicTests : BaseUiTest
{
    [Fact]
    public async Task Should_Load_Page_Successfully()
    {
        // Arrange
        var expectedTitle = "Example Domain";
        Logger.Information("Starting page load test");
        
        // Act
        await Page.GotoAsync("https://example.com");
        var actualTitle = await Page.TitleAsync();
        
        // Assert
        Assert.Equal(expectedTitle, actualTitle);
        Logger.Information("Page loaded successfully with title: {Title}", actualTitle);
    }
}
```

## **Part 3: First Playwright Tests (45 minutes)**

### **Basic Navigation and Assertions**
```csharp
[Fact]
public async Task Should_Navigate_And_Verify_Elements()
{
    // Navigate to page
    await _page.GotoAsync("https://example.com");
    
    // Wait for page to load
    await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    
    // Basic assertions
    await Expect(_page).ToHaveTitleAsync("Example Domain");
    await Expect(_page.Locator("h1")).ToBeVisibleAsync();
    await Expect(_page.Locator("h1")).ToContainTextAsync("Example Domain");
    
    // Check URL
    Assert.Contains("example.com", _page.Url);
}
```

### **Element Interactions**
```csharp
[Fact]
public async Task Should_Interact_With_Form_Elements()
{
    await _page.GotoAsync("https://the-internet.herokuapp.com/login");
    
    // Fill input fields
    await _page.FillAsync("#username", "tomsmith");
    await _page.FillAsync("#password", "SuperSecretPassword!");
    
    // Click button
    await _page.ClickAsync("button[type='submit']");
    
    // Verify success
    await Expect(_page.Locator(".flash.success")).ToBeVisibleAsync();
    await Expect(_page.Locator(".flash.success")).ToContainTextAsync("You logged into a secure area!");
}
```

### **Configuration Setup**
```json
// appsettings.json
{
  "TestSettings": {
    "BaseUrl": "https://the-internet.herokuapp.com",
    "Timeout": 30000,
    "Headless": false,
    "BrowserType": "chromium"
  }
}
```

```csharp
// Configuration helper
public class TestConfiguration
{
    public string BaseUrl { get; set; }
    public int Timeout { get; set; }
    public bool Headless { get; set; }
    public string BrowserType { get; set; }
}
```

---

# 🎭 **Session 2: Page Object Model & Advanced Playwright**
*Duration: 2 hours*

## **Part 1: Page Object Model Implementation (60 minutes)**

### **Base Page Class (UI/Pages/BasePage.cs)**
```csharp
using Microsoft.Playwright;
using atf.Core.Config;
using Serilog;

public abstract class BasePage
{
    protected readonly IPage Page;
    protected readonly ILogger _logger;
    protected abstract string PageUrl { get; }

    protected BasePage(IPage page)
    {
        Page = page;
        _logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
    }

    // Navigate to this page's URL
    public virtual async Task NavigateToAsync()
    {
        var baseUrl = ConfigManager.Get<string>("Playwright:BaseUrl");
        var fullUrl = $"{baseUrl}{PageUrl}";
        
        _logger.Information("Navigating to {Url}", fullUrl);
        await Page.GotoAsync(fullUrl);
        await WaitForPageLoadAsync();
    }

    public async Task WaitForPageLoadAsync()
    {
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        _logger.Information("Page loaded: {Title}", await Page.TitleAsync());
    }

    public async Task<string> GetPageTitleAsync()
    {
        return await Page.TitleAsync();
    }

    // Allure-integrated screenshot helper
    public async Task TakeScreenshotAsync(string name)
    {
        var path = $"screenshots/{name}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
        await Page.ScreenshotAsync(new() { Path = path, FullPage = true });
        
        // Attach to Allure report
        AllureApi.AddAttachment(name, "image/png", File.ReadAllBytes(path));
        _logger.Information("Screenshot taken: {Path}", path);
    }

    // Wait for element with logging
    protected async Task<ILocator> WaitForElementAsync(string selector, int timeout = 30000)
    {
        _logger.Information("Waiting for element: {Selector}", selector);
        var locator = Page.Locator(selector);
        await locator.WaitForAsync(new() { Timeout = timeout });
        return locator;
    }
}
```

### **Login Page Implementation with ILocator**
```csharp
public class LoginPage : BasePage
{
    // Locators as ILocator properties - initialized in constructor
    public ILocator UsernameInput { get; }
    public ILocator PasswordInput { get; }
    public ILocator LoginButton { get; }
    public ILocator ErrorMessage { get; }
    public ILocator SuccessMessage { get; }

    public LoginPage(IPage page, TestConfiguration config) : base(page, config)
    {
        // Initialize locators using Page.Locator()
        UsernameInput = Page.Locator("#username");
        PasswordInput = Page.Locator("#password");
        LoginButton = Page.Locator("button[type='submit']");
        ErrorMessage = Page.Locator(".flash.error");
        SuccessMessage = Page.Locator(".flash.success");
    }

    // Page-specific methods using ILocator directly
    public async Task<LoginPage> NavigateAsync()
    {
        await NavigateToAsync("/login");
        return this;
    }

    public async Task<bool> IsDisplayedAsync()
    {
        return await LoginButton.IsVisibleAsync();
    }

    public async Task<LoginPage> FillUsernameAsync(string username)
    {
        await UsernameInput.FillAsync(username);
        return this; // Fluent interface
    }

    public async Task<LoginPage> FillPasswordAsync(string password)
    {
        await PasswordInput.FillAsync(password);
        return this;
    }

    public async Task<SecureAreaPage> SubmitLoginAsync()
    {
        await LoginButton.ClickAsync();
        return new SecureAreaPage(Page, Config);
    }

    // Login with credentials in one method
    public async Task<SecureAreaPage> LoginAsync(string username, string password)
    {
        await FillUsernameAsync(username);
        await FillPasswordAsync(password);
        return await SubmitLoginAsync();
    }

    public async Task<bool> HasErrorMessageAsync()
    {
        return await ErrorMessage.IsVisibleAsync();
    }

    public async Task<string> GetErrorMessageAsync()
    {
        return await ErrorMessage.TextContentAsync() ?? "";
    }
}
```

### **Secure Area Page with ILocator**
```csharp
public class SecureAreaPage : BasePage
{
    // Locators as ILocator properties
    public ILocator LogoutButton { get; }
    public ILocator SuccessMessage { get; }
    public ILocator PageHeader { get; }

    public SecureAreaPage(IPage page, TestConfiguration config) : base(page, config)
    {
        // Initialize locators
        LogoutButton = Page.Locator("a[href='/logout']");
        SuccessMessage = Page.Locator(".flash.success");
        PageHeader = Page.Locator("h2");
    }

    public async Task<bool> IsDisplayedAsync()
    {
        return await LogoutButton.IsVisibleAsync();
    }

    public async Task<string> GetHeaderTextAsync()
    {
        return await PageHeader.TextContentAsync() ?? "";
    }

    public async Task<bool> HasSuccessMessageAsync()
    {
        return await SuccessMessage.IsVisibleAsync();
    }

    public async Task<LoginPage> LogoutAsync()
    {
        await LogoutButton.ClickAsync();
        return new LoginPage(Page, Config);
    }
}
```

### **Benefits of ILocator-based Page Objects**

**✅ Advantages:**
- **Type Safety**: ILocator provides compile-time safety and IntelliSense
- **Built-in Waiting**: Automatically waits for elements to be ready
- **Rich Methods**: Access to `.ClickAsync()`, `.FillAsync()`, `.IsVisibleAsync()` directly
- **Chaining**: Use `.Filter()`, `.Nth()`, `.First()`, `.Last()` for complex selections
- **No String Duplication**: Locators defined once in constructor
- **Better Performance**: Playwright optimizes locator resolution

**Example of Advanced ILocator Usage:**
```csharp
public class ProductListPage : BasePage
{
    public ILocator ProductCards { get; }
    public ILocator AddToCartButtons { get; }
    public ILocator ProductTitles { get; }

    public ProductListPage(IPage page) : base(page)
    {
        ProductCards = Page.Locator(".product-card");
        AddToCartButtons = Page.Locator(".add-to-cart");
        ProductTitles = Page.Locator(".product-title");
    }

    // Get specific product by index
    public ILocator GetProductCard(int index) => ProductCards.Nth(index);
    
    // Get product by name
    public ILocator GetProductByName(string name) => 
        ProductCards.Filter(new() { Has = ProductTitles.Filter(new() { HasText = name }) });
    
    // Click add to cart for specific product
    public async Task AddToCartByNameAsync(string productName)
    {
        var productCard = GetProductByName(productName);
        await productCard.Locator(".add-to-cart").ClickAsync();
    }
}
```

## **Part 2: Advanced Playwright Operations (60 minutes)**

### **Complex Element Interactions**
```csharp
public class AdvancedInteractionsPage : BasePage
{
    public AdvancedInteractionsPage(IPage page, TestConfiguration config) : base(page, config) { }

    // Dropdown handling
    public async Task SelectDropdownByValueAsync(string selector, string value)
    {
        await Page.SelectOptionAsync(selector, value);
    }

    public async Task SelectDropdownByTextAsync(string selector, string text)
    {
        await Page.SelectOptionAsync(selector, new SelectOptionValue { Label = text });
    }

    // Checkbox and radio buttons
    public async Task CheckCheckboxAsync(string selector)
    {
        await Page.CheckAsync(selector);
    }

    public async Task UncheckCheckboxAsync(string selector)
    {
        await Page.UncheckAsync(selector);
    }

    public async Task<bool> IsCheckedAsync(string selector)
    {
        return await Page.IsCheckedAsync(selector);
    }

    // File uploads
    public async Task UploadFileAsync(string selector, string filePath)
    {
        await Page.SetInputFilesAsync(selector, filePath);
    }

    // Hover and context menus
    public async Task HoverElementAsync(string selector)
    {
        await Page.HoverAsync(selector);
    }

    public async Task RightClickAsync(string selector)
    {
        await Page.ClickAsync(selector, new() { Button = MouseButton.Right });
    }

    // Drag and drop
    public async Task DragAndDropAsync(string sourceSelector, string targetSelector)
    {
        await Page.DragAndDropAsync(sourceSelector, targetSelector);
    }

    // Handle alerts/dialogs
    public async Task HandleAlertAsync(string expectedMessage, bool accept = true)
    {
        Page.Dialog += async (_, dialog) =>
        {
            Assert.Equal(expectedMessage, dialog.Message);
            if (accept)
                await dialog.AcceptAsync();
            else
                await dialog.DismissAsync();
        };
    }

    // Multiple windows/tabs
    public async Task<IPage> ClickAndWaitForNewPageAsync(string selector)
    {
        var newPageTask = Page.Context.WaitForPageAsync();
        await Page.ClickAsync(selector);
        return await newPageTask;
    }

    // Frames handling
    public async Task<IFrame> GetFrameAsync(string nameOrSelector)
    {
        return Page.Frame(nameOrSelector);
    }

    public async Task InteractWithFrameAsync(string frameName, string elementSelector, string text)
    {
        var frame = Page.Frame(frameName);
        await frame.FillAsync(elementSelector, text);
    }
}
```

### **Waiting Strategies**
```csharp
public class WaitingStrategiesExample
{
    private readonly IPage _page;

    public WaitingStrategiesExample(IPage page)
    {
        _page = page;
    }

    // Wait for element to be visible
    public async Task WaitForElementVisibleAsync(string selector, int timeout = 30000)
    {
        await _page.WaitForSelectorAsync(selector, new() 
        { 
            State = WaitForSelectorState.Visible,
            Timeout = timeout 
        });
    }

    // Wait for element to be hidden
    public async Task WaitForElementHiddenAsync(string selector)
    {
        await _page.WaitForSelectorAsync(selector, new() 
        { 
            State = WaitForSelectorState.Hidden 
        });
    }

    // Wait for specific text content
    public async Task WaitForTextAsync(string selector, string expectedText)
    {
        await _page.Locator(selector).Filter(new() { HasText = expectedText }).WaitForAsync();
    }

    // Wait for URL change
    public async Task WaitForUrlAsync(string urlPattern)
    {
        await _page.WaitForURLAsync(urlPattern);
    }

    // Wait for network request
    public async Task<IResponse> WaitForApiCallAsync(string urlPattern)
    {
        return await _page.WaitForResponseAsync(urlPattern);
    }

    // Custom wait condition
    public async Task WaitForCustomConditionAsync(Func<Task<bool>> condition, int timeoutMs = 30000)
    {
        var timeout = TimeSpan.FromMilliseconds(timeoutMs);
        var start = DateTime.Now;

        while (DateTime.Now - start < timeout)
        {
            if (await condition())
                return;
            
            await Task.Delay(500); // Check every 500ms
        }

        throw new TimeoutException($"Condition not met within {timeoutMs}ms");
    }
}
```

### **Data-Driven Tests with Test Data**
```csharp
// Test data model
public class LoginTestData
{
    public string Username { get; set; }
    public string Password { get; set; }
    public bool ShouldSucceed { get; set; }
    public string ExpectedMessage { get; set; }
}

// Theory tests with inline data
public class DataDrivenTests : IAsyncLifetime
{
    private IPage _page;
    private LoginPage _loginPage;

    public async Task InitializeAsync()
    {
        var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync();
        _page = await browser.NewPageAsync();
        _loginPage = new LoginPage(_page, new TestConfiguration());
    }

    [Theory]
    [InlineData("tomsmith", "SuperSecretPassword!", true, "You logged into a secure area!")]
    [InlineData("invaliduser", "wrongpassword", false, "Your username is invalid!")]
    [InlineData("", "password", false, "Your username is invalid!")]
    public async Task Should_Handle_Login_Scenarios(string username, string password, bool shouldSucceed, string expectedMessage)
    {
        // Arrange
        await _loginPage.NavigateAsync();

        // Act
        if (shouldSucceed)
        {
            var securePage = await _loginPage.LoginAsync(username, password);
            
            // Assert
            Assert.True(await securePage.IsDisplayedAsync());
            Assert.True(await securePage.HasSuccessMessageAsync());
        }
        else
        {
            await _loginPage.FillUsernameAsync(username);
            await _loginPage.FillPasswordAsync(password);
            await _loginPage.SubmitLoginAsync();
            
            // Assert
            Assert.True(await _loginPage.HasErrorMessageAsync());
            var errorMessage = await _loginPage.GetErrorMessageAsync();
            Assert.Contains(expectedMessage, errorMessage);
        }
    }

    public async Task DisposeAsync()
    {
        await _page.Context.CloseAsync();
    }
}
```

---

# 🔌 **Session 3: API Testing & Framework Integration**
*Duration: 2 hours*

## **Part 1: API Testing with RestSharp (60 minutes)**

### **API Client Setup with True Factory Pattern**
```csharp
// API Client Factory (API/Clients/ApiClientFactory.cs)
using atf.Core.Config;
using atf.Core.Enums;
using RestSharp;

public static class ApiClientFactory
{
    // True Factory Pattern - returns interface, not concrete type
    public static IApiClient CreateClient(ApiClientType clientType)
    {
        var restClient = CreateRestClient();
        
        return clientType switch
        {
            ApiClientType.Product => new ProductApiClient(restClient),
            ApiClientType.User => new UserApiClient(restClient),
            _ => throw new ArgumentException($"Unsupported client type: {clientType}")
        };
    }
    
    // Convenience methods
    public static IApiClient CreateProductClient() => CreateClient(ApiClientType.Product);
    public static IApiClient CreateUserClient() => CreateClient(ApiClientType.User);
    
    private static RestClient CreateRestClient()
    {
        var baseUrl = ConfigManager.Get<string>("TestApiSettings:BaseUrl");
        var timeout = TimeSpan.FromMilliseconds(ConfigManager.Get<int>("ApiClient:TimeoutMs"));
        
        var options = new RestClientOptions(baseUrl)
        {
            ThrowOnAnyError = false,
            Timeout = timeout
        };

        return new RestClient(options);
    }
}

// Base API Test Class (Tests/Base/BaseApiTest.cs)
public abstract class BaseApiTest<TClient> : IAsyncLifetime, IDisposable
    where TClient : IApiClient
{
    protected TClient Client { get; private set; }
    protected ILogger Logger { get; private set; }

    protected BaseApiTest()
    {
        Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .Enrich.WithProperty("TestContext", "API")
            .CreateLogger();
    }

    protected abstract TClient CreateClient();

    public virtual async Task InitializeAsync()
    {
        Client = CreateClient();
        Logger.Information("API client initialized");
        await Task.CompletedTask;
    }

    public virtual async Task DisposeAsync()
    {
        Client?.Dispose();
        Logger.Information("API client disposed");
    }

    public virtual void Dispose()
    {
        DisposeAsync().GetAwaiter().GetResult();
    }
}

// Product API Test Base (Tests/Base/ProductApiTestBase.cs)
public abstract class ProductApiTestBase : BaseApiTest<IApiClient>
{
    protected override IApiClient CreateClient()
    {
        return ApiClientFactory.CreateProductClient();
    }
}
```

### **API Models**
```csharp
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public Address Address { get; set; }
    public string Phone { get; set; }
    public string Website { get; set; }
    public Company Company { get; set; }
}

public class Address
{
    public string Street { get; set; }
    public string Suite { get; set; }
    public string City { get; set; }
    public string Zipcode { get; set; }
    public Geo Geo { get; set; }
}

public class Geo
{
    public string Lat { get; set; }
    public string Lng { get; set; }
}

public class Company
{
    public string Name { get; set; }
    public string CatchPhrase { get; set; }
    public string Bs { get; set; }
}

public class Post
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
}
```

### **API Test Examples with Real Project Patterns**
```csharp
using atf.Tests.Base;
using atf.API.Models;
using atf.API.Builders;
using atf.Core.Utils;
using Allure.Xunit.Attributes;

[AllureSuite("Product API")]
public class ProductApiTests : ProductApiTestBase
{
    [Fact]
    [AllureFeature("Product Creation")]
    public async Task Should_Create_Product_Successfully()
    {
        // Arrange - Using Builder pattern with fake data
        var productRequest = new ProductRequestBuilder()
            .WithFakeData()  // Uses TestDataFaker internally
            .Build();
        
        Logger.Information("Creating product: {@ProductRequest}", productRequest);

        // Act
        var createdProduct = await Client.PostAsync<ProductRequest>(productRequest, "/api/products");

        // Assert
        Assert.NotNull(createdProduct);
        Assert.Equal(productRequest.Name, createdProduct.Name);
        Assert.Equal(productRequest.Category, createdProduct.Category);
        Assert.True(createdProduct.Price > 0);
        
        Logger.Information("Product created successfully with ID: {ProductId}", createdProduct.Id);
        
        // Attach response to Allure report
        AllureHelper.AttachString("Response Body", createdProduct.ToString(), "application/json", ".json");
    }

    [Fact]
    [AllureFeature("Product Retrieval")]
    public async Task Should_Get_Product_By_Id()
    {
        // Arrange
        var productId = 1;
        Logger.Information("Retrieving product with ID: {ProductId}", productId);

        // Act
        var product = await Client.GetAsync<ProductRequest>($"/api/products/{productId}");

        // Assert
        Assert.NotNull(product);
        Assert.Equal(productId, product.Id);
        Assert.NotEmpty(product.Name);
        
        Logger.Information("Product retrieved: {ProductName}", product.Name);
    }

    [Theory]
    [InlineData("Electronics")]
    [InlineData("Clothing")]
    [InlineData("Books")]
    [AllureFeature("Product Search")]
    public async Task Should_Get_Products_By_Category(string category)
    {
        // Arrange
        Logger.Information("Searching products in category: {Category}", category);

        // Act
        var products = await Client.GetAsync<List<ProductRequest>>($"/api/products?category={category}");

        // Assert
        Assert.NotNull(products);
        Assert.All(products, p => Assert.Equal(category, p.Category));
        
        Logger.Information("Found {Count} products in category {Category}", products.Count, category);
    }
}

// User API Tests Example
[AllureSuite("User API")]
public class UserApiTests : UserApiTestBase
{
    [Fact]
    [AllureFeature("User Authentication")]
    public async Task Should_Login_With_Valid_Credentials()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Username = "testuser",
            Password = "password123"
        };
        
        Logger.Information("Attempting login for user: {Username}", loginRequest.Username);

        // Act
        var loginResponse = await Client.PostAsync<LoginResponse>(loginRequest, "/api/users/login");

        // Assert
        Assert.NotNull(loginResponse);
        Assert.Equal("Login successful", loginResponse.Message);
        Assert.NotNull(loginResponse.Role);
        
        Logger.Information("User logged in successfully with role: {Role}", loginResponse.Role);
    }

    [Fact]
    public async Task Should_Get_User_By_Id()
    {
        // Arrange
        var userId = 1;
        var request = new RestRequest($"/users/{userId}", Method.Get);

        // Act
        var response = await Client.ExecuteAsync<User>(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.Equal(userId, response.Data.Id);
        Assert.Equal("Leanne Graham", response.Data.Name);
    }

    [Fact]
    public async Task Should_Create_New_Post()
    {
        // Arrange
        var newPost = new Post
        {
            UserId = 1,
            Title = "Workshop Test Post",
            Body = "This is a test post created during the workshop"
        };
        
        var request = new RestRequest("/posts", Method.Post);
        request.AddJsonBody(newPost);

        // Act
        var response = await Client.ExecuteAsync<Post>(request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.Equal(101, response.Data.Id); // JSONPlaceholder returns 101 for new posts
        Assert.Equal(newPost.Title, response.Data.Title);
        Assert.Equal(newPost.Body, response.Data.Body);
    }

    [Fact]
    public async Task Should_Update_Existing_Post()
    {
        // Arrange
        var postId = 1;
        var updatedPost = new Post
        {
            Id = postId,
            UserId = 1,
            Title = "Updated Workshop Post",
            Body = "This post has been updated during the workshop"
        };
        
        var request = new RestRequest($"/posts/{postId}", Method.Put);
        request.AddJsonBody(updatedPost);

        // Act
        var response = await Client.ExecuteAsync<Post>(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.Equal(postId, response.Data.Id);
        Assert.Equal(updatedPost.Title, response.Data.Title);
    }

    [Fact]
    public async Task Should_Delete_Post()
    {
        // Arrange
        var postId = 1;
        var request = new RestRequest($"/posts/{postId}", Method.Delete);

        // Act
        var response = await Client.ExecuteAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Should_Handle_Not_Found_Error()
    {
        // Arrange
        var invalidUserId = 999;
        var request = new RestRequest($"/users/{invalidUserId}", Method.Get);

        // Act
        var response = await Client.ExecuteAsync<User>(request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Null(response.Data);
    }
}
```

### **API Helper Methods**
```csharp
public static class ApiHelpers
{
    public static async Task<T> ExecuteAndValidateAsync<T>(RestClient client, RestRequest request, HttpStatusCode expectedStatus = HttpStatusCode.OK)
    {
        var response = await client.ExecuteAsync<T>(request);
        
        Assert.Equal(expectedStatus, response.StatusCode);
        Assert.NotNull(response.Data);
        
        return response.Data;
    }

    public static async Task<RestResponse> ExecuteAndValidateAsync(RestClient client, RestRequest request, HttpStatusCode expectedStatus = HttpStatusCode.OK)
    {
        var response = await client.ExecuteAsync(request);
        
        Assert.Equal(expectedStatus, response.StatusCode);
        
        return response;
    }

    public static RestRequest CreateJsonRequest(string resource, Method method, object body = null)
    {
        var request = new RestRequest(resource, method);
        
        if (body != null)
        {
            request.AddJsonBody(body);
        }
        
        return request;
    }
}
```

## **Part 2: Integrated Testing & Framework Features (60 minutes)**

### **Combined UI + API Test**
```csharp
public class IntegratedTests : IAsyncLifetime
{
    private IPage _page;
    private RestClient _apiClient;
    private LoginPage _loginPage;

    public async Task InitializeAsync()
    {
        // Setup UI
        var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync();
        _page = await browser.NewPageAsync();
        _loginPage = new LoginPage(_page, new TestConfiguration());

        // Setup API
        _apiClient = new RestClient("https://jsonplaceholder.typicode.com");
    }

    [Fact]
    public async Task Should_Verify_User_Data_Via_UI_And_API()
    {
        // Step 1: Get user data via API
        var apiRequest = new RestRequest("/users/1", Method.Get);
        var apiResponse = await _apiClient.ExecuteAsync<User>(apiRequest);
        var userData = apiResponse.Data;

        // Step 2: Login via UI
        await _loginPage.NavigateAsync();
        var securePage = await _loginPage.LoginAsync("tomsmith", "SuperSecretPassword!");

        // Step 3: Verify UI shows correct user information
        Assert.True(await securePage.IsDisplayedAsync());
        
        // This is a conceptual example - in real scenarios, you'd verify 
        // that the UI displays the same data that the API returned
        var displayedUserInfo = await _page.Locator(".user-info").TextContentAsync();
        // Assert.Contains(userData.Name, displayedUserInfo);
    }

    public async Task DisposeAsync()
    {
        await _page.Context.CloseAsync();
        _apiClient?.Dispose();
    }
}
```

### **Test Data Management**
```csharp
// testdata.json
{
  "users": [
    {
      "username": "admin",
      "password": "admin123",
      "role": "administrator"
    },
    {
      "username": "user1",
      "password": "user123",
      "role": "standard"
    }
  ],
  "testUrls": {
    "login": "/login",
    "dashboard": "/dashboard",
    "profile": "/profile"
  }
}
```

```csharp
public class TestDataManager
{
    private static readonly string TestDataPath = "testdata.json";
    
    public static T LoadTestData<T>(string section = null)
    {
        var json = File.ReadAllText(TestDataPath);
        var data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);
        
        if (section != null)
        {
            return JsonSerializer.Deserialize<T>(data[section].GetRawText());
        }
        
        return JsonSerializer.Deserialize<T>(json);
    }
}

// Usage in tests
[Fact]
public async Task Should_Login_With_Test_Data()
{
    var users = TestDataManager.LoadTestData<List<TestUser>>("users");
    var adminUser = users.First(u => u.Role == "administrator");
    
    await _loginPage.NavigateAsync();
    var securePage = await _loginPage.LoginAsync(adminUser.Username, adminUser.Password);
    
    Assert.True(await securePage.IsDisplayedAsync());
}
```

### **Allure Reporting Integration**
```csharp
// Install Allure packages
// dotnet add package Allure.Xunit
// dotnet add package Allure.Net.Commons

using Allure.Xunit;
using Allure.Xunit.Attributes;
using Allure.Net.Commons;
using atf.Tests.Helpers;

[AllureSuite("Login Tests")]
[AllureFeature("User Authentication")]
public class LoginTests : BaseUiTest
{
    [Fact(DisplayName = "Should login with valid credentials")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureOwner("QA Team")]
    [AllureTag("smoke")]
    public async Task Should_Login_Successfully()
    {
        // Arrange
        var testName = nameof(Should_Login_Successfully);
        Logger.Information("Starting test: {TestName}", testName);
        
        // Act
        await Page.GotoAsync("https://the-internet.herokuapp.com/login");
        await Page.FillAsync("#username", "tomsmith");
        await Page.FillAsync("#password", "SuperSecretPassword!");
        
        // Take screenshot before action
        await TakeScreenshotAsync("before-login");
        AllureHelper.AttachString("Login Credentials", "Username: tomsmith");
        
        await Page.ClickAsync("button[type='submit']");
        
        // Assert
        await Expect(Page.Locator(".flash.success")).ToBeVisibleAsync();
        await TakeScreenshotAsync("after-login");
        
        Logger.Information("Test completed successfully: {TestName}", testName);
    }

    [Fact(DisplayName = "Should handle invalid credentials")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureTag("negative")]
    public async Task Should_Handle_Invalid_Login()
    {
        // This test will fail and show screenshot in Allure report
        await Page.GotoAsync("https://the-internet.herokuapp.com/login");
        await Page.FillAsync("#username", "invaliduser");
        await Page.FillAsync("#password", "wrongpassword");
        await Page.ClickAsync("button[type='submit']");
        
        // Screenshot on failure will be automatically attached
        await Expect(Page.Locator(".flash.error")).ToBeVisibleAsync();
        
        var errorMessage = await Page.Locator(".flash.error").TextContentAsync();
        AllureHelper.AttachString("Error Message", errorMessage);
    }
}

// Allure Helper (Tests/Helpers/AllureHelper.cs)
public static class AllureHelper
{
    public static void AttachString(string name, object content, string type = "text/plain", string fileExtension = ".txt")
    {
        var json = content is string str ? str : JsonSerializer.Serialize(content, new JsonSerializerOptions { WriteIndented = true });
        AllureApi.AddAttachment(name, type, Encoding.UTF8.GetBytes(json), fileExtension);
    }

    public static void AttachScreenshot(string path, string name = "Screenshot")
    {
        if (File.Exists(path))
        {
            AllureApi.AddAttachment(name, "image/png", File.ReadAllBytes(path));
        }
    }
}
```

### **Enhanced Test Base with Allure Integration**
```csharp
public abstract class BaseUiTest : IAsyncLifetime
{
    protected IPage Page;
    protected IBrowser Browser;
    protected ILogger Logger;

    public async Task InitializeAsync()
    {
        // Browser setup with Allure environment info
        var playwright = await Playwright.CreateAsync();
        Browser = await playwright.Chromium.LaunchAsync(new()
        {
            Headless = !Debugger.IsAttached
        });
        
        Page = await Browser.NewPageAsync();
        
        // Add environment info to Allure
        AllureLifecycle.Instance.UpdateTestCase(testCase =>
        {
            testCase.labels.Add(new Label { name = "Browser", value = "Chromium" });
            testCase.labels.Add(new Label { name = "Environment", value = "Test" });
        });
    }

    protected async Task TakeScreenshotAsync(string name)
    {
        var path = $"screenshots/{name}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
        await Page.ScreenshotAsync(new() { Path = path, FullPage = true });
        
        // Automatically attach to Allure
        AllureHelper.AttachScreenshot(path, name);
        Logger.Information("Screenshot attached to Allure: {Name}", name);
    }

    public async Task DisposeAsync()
    {
        // Auto-screenshot on test failure
        var testContext = AllureLifecycle.Instance.Context;
        if (testContext.HasAny() && 
            AllureLifecycle.Instance.Context.Get().status == Status.failed)
        {
            await TakeScreenshotAsync("test-failure");
        }
        
        await Browser?.CloseAsync();
    }
}
```

### **Running Tests with Allure Reports**
```bash
# Run tests (generates results in allure-results/)
dotnet test --logger allure

# Generate and serve Allure report
./run-allure-tests.ps1

# Or manually:
allure generate allure-results -o allure-report --clean
allure serve allure-results
```

### **Configuration for Different Environments**
```csharp
// appsettings.Development.json
{
  "TestSettings": {
    "BaseUrl": "https://dev.example.com",
    "ApiBaseUrl": "https://api-dev.example.com",
    "Headless": false,
    "SlowMo": 1000
  }
}

// appsettings.Production.json
{
  "TestSettings": {
    "BaseUrl": "https://prod.example.com",
    "ApiBaseUrl": "https://api.example.com",
    "Headless": true,
    "SlowMo": 0
  }
}
```

### **Parallel Test Execution**
```csharp
// xunit.runner.json
{
  "parallelizeAssembly": true,
  "parallelizeTestCollections": true,
  "maxParallelThreads": 4
}

// Collection definition for tests that can't run in parallel
[CollectionDefinition("Sequential")]
public class SequentialCollection : ICollectionFixture<SequentialTestFixture>
{
}

[Collection("Sequential")]
public class DatabaseTests
{
    // These tests will run sequentially
}
```

## **Part 2: Integrated Testing & Basic Reporting (60 minutes)**

### **Combined UI + API Test**
```csharp
public class IntegratedTests : IAsyncLifetime
{
    private IPage _page;
    private RestClient _apiClient;
    private LoginPage _loginPage;

    public async Task InitializeAsync()
    {
        // Setup UI
        var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync();
        _page = await browser.NewPageAsync();
        _loginPage = new LoginPage(_page);

        // Setup API
        _apiClient = new RestClient("https://jsonplaceholder.typicode.com");
    }

    [Fact]
    public async Task Should_Verify_User_Data_Via_UI_And_API()
    {
        // Step 1: Get user data via API
        var apiRequest = new RestRequest("/users/1", Method.Get);
        var apiResponse = await _apiClient.ExecuteAsync<User>(apiRequest);
        var userData = apiResponse.Data;

        // Step 2: Login via UI (if needed)
        await _loginPage.NavigateAsync();
        // Verify UI displays correct information

        Assert.NotNull(userData);
        Assert.Equal("Leanne Graham", userData.Name);
    }

    public async Task DisposeAsync()
    {
        await _page.Context.CloseAsync();
        _apiClient?.Dispose();
    }
}
```

### **Simple Test Data Management**
```csharp
// Simple test data without complex patterns
public class TestData
{
    public static User CreateTestUser()
    {
        return new User
        {
            Name = "Test User",
            Email = "test@example.com",
            Username = "testuser"
        };
    }

    public static Post CreateTestPost()
    {
        return new Post
        {
            UserId = 1,
            Title = "Test Post",
            Body = "This is a test post"
        };
    }
}
```

### **Basic Allure Reporting**
```csharp
// Simple Allure integration
[Fact]
public async Task Should_Login_Successfully()
{
    // Take screenshot before action
    await Page.ScreenshotAsync(new() { Path = "before-login.png" });
    
    await Page.GotoAsync("https://the-internet.herokuapp.com/login");
    await Page.FillAsync("#username", "tomsmith");
    await Page.FillAsync("#password", "SuperSecretPassword!");
    await Page.ClickAsync("button[type='submit']");
    
    // Take screenshot after action
    await Page.ScreenshotAsync(new() { Path = "after-login.png" });
    
    await Expect(Page.Locator(".flash.success")).ToBeVisibleAsync();
}
```

### **Running Tests with Reports**
```bash
# Run tests with Allure
dotnet test --logger:allure

# Generate and view report
allure generate allure-results -o allure-report --clean
allure serve allure-results
```

---

## 🎯 **Workshop Exercises**

### **Session 1 Exercises**
1. **Environment Setup**: Set up .NET project with Playwright
2. **First Test**: Write a test that navigates to a website and verifies the title
3. **Form Interaction**: Create a test that fills out and submits a login form
4. **Configuration**: Add appsettings.json and read configuration in tests

### **Session 2 Exercises**
1. **Page Objects**: Convert direct Playwright calls to Page Object Model
2. **Advanced Interactions**: Implement tests for dropdowns, checkboxes, file uploads
3. **Data-Driven**: Create parameterized tests with multiple test data sets
4. **Error Handling**: Add proper error handling and screenshot capture

### **Session 3 Exercises**
1. **API Tests**: Write tests for GET, POST, PUT, DELETE operations
2. **Integration**: Create a test that combines UI and API validation
3. **Test Data**: Create simple test data helpers for users and posts
4. **Allure Setup**: Configure basic Allure reporting with screenshots

---

## 📚 **Additional Resources**

### **Documentation Links**
- [.NET Official Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Playwright for .NET](https://playwright.dev/dotnet/)
- [xUnit Documentation](https://xunit.net/)
- [RestSharp Documentation](https://restsharp.dev/)

### **Practice Websites**
- [The Internet](https://the-internet.herokuapp.com/) - Various UI testing scenarios
- [JSONPlaceholder](https://jsonplaceholder.typicode.com/) - Fake API for testing
- [TestingChallenges](https://testingchallenges.thetestingmap.org/) - UI challenges
- [ReqRes](https://reqres.in/) - Test API with different response types

### **Next Steps**
1. **Advanced Patterns**: Builder pattern for test data, Factory pattern for page objects
2. **Performance Testing**: Load testing with NBomber
3. **Visual Testing**: Screenshot comparison with Playwright
4. **Mobile Testing**: Mobile browser testing with Playwright
5. **Database Testing**: Entity Framework integration
6. **Reporting**: Allure reporting integration

---

*Happy Testing! 🎭✨*