# .NET Playwright Workshop Program

*A 3-session hands-on workshop for programmers transitioning to .NET test automation*

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
| **Session 3** | API Testing & Framework Integration | 2 hours | RestSharp, data-driven tests, CI/CD |

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

### **xUnit Test Structure**
```csharp
using Xunit;
using Microsoft.Playwright;

public class BasicTests : IAsyncLifetime
{
    private IBrowser _browser;
    private IPage _page;

    // Setup - runs before each test
    public async Task InitializeAsync()
    {
        var playwright = await Playwright.CreateAsync();
        _browser = await playwright.Chromium.LaunchAsync(new()
        {
            Headless = false, // Set to true for CI
            SlowMo = 1000     // Slow down for demo
        });
        _page = await _browser.NewPageAsync();
    }

    // Cleanup - runs after each test
    public async Task DisposeAsync()
    {
        await _browser.CloseAsync();
    }

    [Fact]
    public async Task Should_Load_Page_Successfully()
    {
        // Arrange
        var expectedTitle = "Example Domain";
        
        // Act
        await _page.GotoAsync("https://example.com");
        var actualTitle = await _page.TitleAsync();
        
        // Assert
        Assert.Equal(expectedTitle, actualTitle);
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

### **Base Page Class**
```csharp
public abstract class BasePage
{
    protected readonly IPage Page;
    protected readonly TestConfiguration Config;

    protected BasePage(IPage page, TestConfiguration config)
    {
        Page = page;
        Config = config;
    }

    // Common methods for all pages
    public async Task NavigateToAsync(string relativeUrl)
    {
        var fullUrl = $"{Config.BaseUrl}{relativeUrl}";
        await Page.GotoAsync(fullUrl);
        await WaitForPageLoadAsync();
    }

    public async Task WaitForPageLoadAsync()
    {
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task<string> GetPageTitleAsync()
    {
        return await Page.TitleAsync();
    }

    // Screenshot helper
    public async Task TakeScreenshotAsync(string name)
    {
        await Page.ScreenshotAsync(new()
        {
            Path = $"screenshots/{name}_{DateTime.Now:yyyyMMdd_HHmmss}.png",
            FullPage = true
        });
    }
}
```

### **Login Page Implementation**
```csharp
public class LoginPage : BasePage
{
    // Locators - centralized element selectors
    private readonly string _usernameInput = "#username";
    private readonly string _passwordInput = "#password";
    private readonly string _loginButton = "button[type='submit']";
    private readonly string _errorMessage = ".flash.error";
    private readonly string _successMessage = ".flash.success";

    public LoginPage(IPage page, TestConfiguration config) : base(page, config) { }

    // Page-specific methods
    public async Task<LoginPage> NavigateAsync()
    {
        await NavigateToAsync("/login");
        return this;
    }

    public async Task<bool> IsDisplayedAsync()
    {
        return await Page.Locator(_loginButton).IsVisibleAsync();
    }

    public async Task<LoginPage> FillUsernameAsync(string username)
    {
        await Page.FillAsync(_usernameInput, username);
        return this; // Fluent interface
    }

    public async Task<LoginPage> FillPasswordAsync(string password)
    {
        await Page.FillAsync(_passwordInput, password);
        return this;
    }

    public async Task<SecureAreaPage> SubmitLoginAsync()
    {
        await Page.ClickAsync(_loginButton);
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
        return await Page.Locator(_errorMessage).IsVisibleAsync();
    }

    public async Task<string> GetErrorMessageAsync()
    {
        return await Page.Locator(_errorMessage).TextContentAsync();
    }
}
```

### **Secure Area Page**
```csharp
public class SecureAreaPage : BasePage
{
    private readonly string _logoutButton = "a[href='/logout']";
    private readonly string _successMessage = ".flash.success";
    private readonly string _pageHeader = "h2";

    public SecureAreaPage(IPage page, TestConfiguration config) : base(page, config) { }

    public async Task<bool> IsDisplayedAsync()
    {
        return await Page.Locator(_logoutButton).IsVisibleAsync();
    }

    public async Task<string> GetHeaderTextAsync()
    {
        return await Page.Locator(_pageHeader).TextContentAsync();
    }

    public async Task<bool> HasSuccessMessageAsync()
    {
        return await Page.Locator(_successMessage).IsVisibleAsync();
    }

    public async Task<LoginPage> LogoutAsync()
    {
        await Page.ClickAsync(_logoutButton);
        return new LoginPage(Page, Config);
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

### **API Client Setup**
```csharp
// Install RestSharp package
// dotnet add package RestSharp

using RestSharp;
using System.Text.Json;

public class ApiTestBase : IAsyncLifetime
{
    protected RestClient Client;
    protected readonly string BaseUrl = "https://jsonplaceholder.typicode.com";

    public async Task InitializeAsync()
    {
        var options = new RestClientOptions(BaseUrl)
        {
            ThrowOnAnyError = false,
            Timeout = TimeSpan.FromSeconds(30)
        };
        
        Client = new RestClient(options);
        await Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        Client?.Dispose();
        await Task.CompletedTask;
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

### **API Test Examples**
```csharp
public class ApiTests : ApiTestBase
{
    [Fact]
    public async Task Should_Get_All_Users()
    {
        // Arrange
        var request = new RestRequest("/users", Method.Get);

        // Act
        var response = await Client.ExecuteAsync<List<User>>(request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.Equal(10, response.Data.Count);
        Assert.All(response.Data, user => Assert.NotEmpty(user.Name));
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

### **Reporting and Screenshots**
```csharp
public class TestBase : IAsyncLifetime
{
    protected IPage Page;
    private IBrowser _browser;

    public async Task InitializeAsync()
    {
        var playwright = await Playwright.CreateAsync();
        _browser = await playwright.Chromium.LaunchAsync(new()
        {
            Headless = !Debugger.IsAttached // Run headless in CI, headed when debugging
        });
        
        Page = await _browser.NewPageAsync();
    }

    protected async Task TakeScreenshotOnFailureAsync([CallerMemberName] string testName = "")
    {
        try
        {
            await Page.ScreenshotAsync(new()
            {
                Path = $"screenshots/failure_{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png",
                FullPage = true
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to take screenshot: {ex.Message}");
        }
    }

    public async Task DisposeAsync()
    {
        // Take screenshot if test failed
        var testResult = TestContext.CurrentContext?.Result?.Outcome;
        if (testResult == TestOutcome.Failed)
        {
            await TakeScreenshotOnFailureAsync();
        }
        
        await _browser.CloseAsync();
    }
}
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
3. **Test Data**: Implement JSON-based test data management
4. **CI/CD Ready**: Configure tests for parallel execution and CI/CD

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