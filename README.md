# .NET Test Automation Framework

*Sponsored by coffee ☕, AI 🤖, and bench 💺*

A comprehensive test automation framework built with **.NET 9** for API, UI, and database testing. Designed with clean architecture principles and follows industry best practices for maintainable and scalable test automation.

## 🚀 Features

- **Multi-Layer Testing**: API, UI, and Database test support with integrated analytics testing
- **Cross-Browser Testing**: Playwright integration with Chrome, Firefox, Safari, and Edge
- **Modern Page Object Model**: Dual approaches with selector-based constants and ILocator patterns
- **Advanced API Testing**: RestSharp-based HTTP client with factory pattern and analytics integration
- **Database Testing**: Entity Framework Core 9.0 with SQL Server and In-Memory providers
- **Professional Reporting**: Comprehensive Allure system with automated archiving and history preservation
- **Report Management**: Professional scripts for archiving, cleaning, and serving reports
- **Playwright Tracing**: Built-in trace capture for visual debugging and test analysis
- **Thread-Safe Logging**: TestLogger with per-class file logging and console output
- **Static Configuration**: Simple ConfigManager for direct configuration access without DI complexity
- **Fake Test Data**: Bogus library integration for realistic test data generation
- **Developer Tooling**: Enhanced Playwright codegen and trace viewer integration
- **CI/CD Ready**: GitHub Actions and Azure DevOps pipeline examples
- **Clean Architecture**: Layered design with clear separation of concerns
- **Cross-Platform Scripts**: Separate Windows and Unix/Mac script versions

## 🏗️ Architecture

### Design Patterns
- **Page Object Model** for UI test organization (dual approach: selector-based and ILocator)
- **Repository Pattern** for data access abstraction
- **Builder Pattern** for test data and request construction
- **Factory Pattern** for API clients and database initialization
- **Template Method** for base test classes

### SOLID Principles
- ✅ **Single Responsibility**: Each class has a focused purpose
- ✅ **Open/Closed**: Extensible base classes without modification
- ✅ **Liskov Substitution**: Proper inheritance hierarchies
- ✅ **Interface Segregation**: Focused interfaces where needed
- ✅ **Dependency Inversion**: Abstractions over concrete implementations

## 🛠️ Technologies

| Category | Technology | Version | Purpose |
|----------|------------|---------|---------|
| **Framework** | .NET | 9.0 | Core platform |
| **Testing** | xUnit | 2.9.3 | Test runner and assertions |
| **UI Automation** | Playwright | 1.52.0 | Cross-browser automation |
| **API Testing** | RestSharp | 112.1.0 | HTTP client and requests |
| **Database** | Entity Framework Core | 9.0.5 | ORM and database testing |
| **Logging** | Serilog | 4.3.0 | Structured logging |
| **Reporting** | Allure | 2.12.1 | Test reporting and analytics |
| **Configuration** | Microsoft.Extensions.Configuration | 9.0 | Settings management |
| **Test Data** | Bogus | 35.6.3 | Fake test data generation |
| **Mocking** | AutoFixture.AutoMoq | 4.18.1 | Mock object generation |

## 📁 Project Structure

```
├── API/                           # API Testing Layer
│   ├── Builders/                  # Request builders (Fluent API)
│   │   └── ProductRequestBuilder.cs
│   ├── Clients/                   # HTTP client wrappers
│   │   ├── ApiClientFactory.cs    # Factory for creating API clients
│   │   ├── AnalyticsClient.cs     # Analytics API client (new)
│   │   ├── BaseClient.cs          # Base HTTP client with common operations
│   │   └── ProductApiClient.cs    # Product-specific API client
│   ├── Fixtures/                  # xUnit test fixtures
│   │   ├── ApiTestFixture.cs
│   │   └── DefaultApiTestFixture.cs
│   └── Models/                    # API request/response models
│       ├── AnalyticsResponse.cs   # Analytics data structure (new)
│       ├── MergedProduct.cs       # Product merger operations (new)
│       ├── Product.cs             # Base product model (new)
│       ├── ProductRequest.cs      # Product request model (refactored)
│       └── TopProduct.cs          # Product with quantity tracking (new)
│
├── Core/                          # Cross-cutting Concerns
│   ├── Config/                    # Configuration management
│   │   └── ConfigManager.cs       # Static configuration manager
│   ├── Enums/                     # Framework enumerations
│   │   ├── ApiClientType.cs       # API client type enumeration (updated)
│   │   └── BrowserList.cs
│   ├── Logging/                   # Logging utilities
│   │   └── TestLogger.cs          # Thread-safe test logging
│   ├── Models/                    # Framework models
│   │   ├── BaseModel.cs
│   │   └── PlaywrightSettings.cs  # Enhanced with tracing support
│   └── Utils/                     # Helper utilities
│       ├── JsonHelper.cs
│       └── TestDataFaker.cs       # Fake test data generation (updated)
│
├── Data/                          # Data Access Layer
│   ├── DatabaseContext/           # Database context management
│   │   ├── DbContextFactory.cs
│   │   └── TestDbContext.cs       # Updated for new entity structure
│   ├── Models/                    # Entity models
│   │   ├── ProductEntity.cs       # Renamed from Product.cs (new)
│   │   └── User.cs
│   └── Repositories/              # Data access repositories
│       ├── IProductRepository.cs  # Updated for ProductEntity
│       ├── IUserRepository.cs
│       ├── ProductRepository.cs   # Updated for ProductEntity
│       └── UserRepository.cs
│
├── Tests/                         # Test Implementation
│   ├── Base/                      # Base test classes (restructured)
│   │   ├── API/                   # API test base classes
│   │   │   ├── AnalyticsBase.cs   # Analytics API test base (new)
│   │   │   ├── BaseApiTest.cs     # Generic API test base
│   │   │   └── ProductApiTestBase.cs # Product-specific API test base
│   │   └── UI/                    # UI test base classes
│   │       └── BaseUiTest.cs      # Enhanced with tracing support
│   ├── Helpers/                   # Test helper utilities
│   │   ├── AllureHelper.cs
│   │   └── DataBaseTestHelper.cs  # Updated for new entity structure
│   ├── Tests.API/                 # API test implementations
│   │   ├── AnalyticsTest.cs       # Analytics API tests (new)
│   │   └── ProductApiTests.cs     # Product API test cases
│   ├── Tests.Mocks/               # Mock and test data providers
│   │   ├── BaseMockProvider.cs
│   │   └── ProductServiceTest.cs
│   └── Tests.UI/                  # UI test implementations
│       ├── AdvancedPageTests.cs   # Advanced UI interactions
│       ├── BaseUrlLaunchTest.cs   # Cross-browser smoke tests
│       └── LoginToProductPage.cs  # End-to-end user flow (new)
│
├── UI/                            # UI Testing Layer
│   ├── Pages/                     # Page Object Models (dual approach)
│   │   ├── AdvancedWebElementsPage.cs # Advanced interactions using selectors
│   │   ├── BasePage.cs            # Base page with common functionality
│   │   ├── BasePageILocator.cs    # ILocator-based base page (new)
│   │   ├── LoginPage.cs           # Login page with ILocator pattern (new)
│   │   ├── UserPage.cs            # User page with complex interactions (new)
│   │   └── WebElementsPage.cs     # Basic page with selector constants
│   └── PlaywrightSetup/           # Browser initialization
│       └── PlaywrightSetup.cs     # Enhanced with tracing integration
│
├── Documentation/                 # Project documentation
│   ├── design-patterns.md         # Architecture patterns guide
│   ├── from-java-cheatsheet.md   # Java to .NET transition guide
│   ├── playwright-getby-methods.md # Modern Playwright selector guide (new)
│   ├── playwright-selectors-cheatsheet.md # Comprehensive selector reference
│   └── workshop-program.md       # Training workshop materials
│
├── reports/                      # Test Execution Reports System
│   ├── allure-results/           # Live test results and history
│   ├── archive/                  # Archived reports with metadata
│   └── scripts/                  # Report management utilities
│       ├── archive-reports.ps1   # Archive reports with build info (Windows)
│       ├── archive-reports-unix.ps1 # Archive reports (Unix/Mac)
│       ├── clean-reports.ps1     # Clean reports (preserve history) (Windows)
│       ├── clean-reports-unix.ps1 # Clean reports (Unix/Mac)
│       ├── list-archives.ps1     # List and examine archives (Windows)
│       ├── list-archives-unix.ps1 # List archives (Unix/Mac)
│       ├── open-report.ps1       # Serve reports via web server (Windows)
│       └── open-report-unix.ps1  # Serve reports (Unix/Mac)
│
├── allureConfig.json             # Allure reporting configuration
├── codegen-unix.ps1              # Enhanced Playwright codegen tool (cross-platform)
├── run-allure-tests.ps1         # Main test execution script (Windows)
├── run-allure-tests-unix.ps1    # Main test execution script (Unix/Mac)
├── testsettings.runsettings     # XUnit parallel execution settings
└── trace-viewer.ps1             # Playwright trace viewer tool (new)
```

## 🖥️ Cross-Platform Support

The framework provides separate execution scripts for Windows and Unix-based systems (macOS, Linux):

### Windows Scripts
- `run-allure-tests.ps1` - Main test execution
- `reports/scripts/*.ps1` - Report management scripts

### Unix/Mac/Linux Scripts  
- `run-allure-tests-unix.ps1` - Main test execution
- `reports/scripts/*-unix.ps1` - Report management scripts
- `codegen-unix.ps1` - Enhanced Playwright codegen
- `trace-viewer.ps1` - Cross-platform trace viewer

The Unix scripts use forward slashes for paths and Unix-specific commands for optimal compatibility.

## ⚙️ Setup

### Prerequisites
- **.NET 9 SDK** or later
- **PowerShell** (for cross-platform script execution)
- **Visual Studio 2022** or **JetBrains Rider** (recommended)
- **SQL Server LocalDB** (for database tests)

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd netframework
   ```

2. **Restore packages**
   ```bash
   dotnet restore
   ```

3. **Install Playwright browsers**
   ```bash
   pwsh bin/Debug/net9.0/playwright.ps1 install
   ```

4. **Setup database** (optional)
   ```bash
   dotnet ef database update
   ```

## 🔧 Configuration

### appsettings.json
```json
{
  "Playwright": {
    "BrowserType": "Firefox",
    "Headless": false,
    "BaseUrl": "http://localhost:3000/",
    "BaseApiHost": "http://localhost:5001",
    "DefaultTimeoutMs": 30000,
    "ViewportWidth": 1280,
    "ViewportHeight": 800,
    "SlowMoMs": 1000,
    "EnableTracing": true
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TestDb_Tests;Trusted_Connection=true;"
  },
  "ApiClient": {
    "TimeoutMs": 30000,
    "RetryAttempts": 3
  },
  "TestApiSettings": {
    "BaseUrl": "http://localhost:5001"
  }
}
```

### Environment Overrides
- Create `appsettings.Development.json` for local settings
- Use environment variables: `Playwright__BrowserType=Chrome`
- Toggle tracing: `Playwright__EnableTracing=true`

## 🧪 Running Tests

### Quick Test Execution
```bash
# Windows
.\run-allure-tests.ps1

# Unix/Mac/Linux
pwsh ./run-allure-tests-unix.ps1

# This will:
# 1. Clean old results while preserving history
# 2. Run smoke tests (filter Category=Smoke)
# 3. Generate fresh Allure report
# 4. Start web server on http://localhost:5000
```

### Specific Test Categories
```bash
# API Tests only
dotnet test --filter Category=API

# UI Tests only
dotnet test --filter Category=UI

# Database Tests only
dotnet test --filter Category=Database

# Smoke tests for CI/CD
dotnet test --filter Category=Smoke
```

### Cross-Browser Testing
```bash
# Override browser via environment
$env:Playwright__BrowserType="Chrome"; dotnet test
$env:Playwright__BrowserType="Safari"; dotnet test
$env:Playwright__Headless="true"; dotnet test
```

### Tracing Control
```bash
# Enable tracing globally
$env:Playwright__EnableTracing="true"; dotnet test

# Disable tracing
$env:Playwright__EnableTracing="false"; dotnet test

# View traces after test execution
pwsh ./trace-viewer.ps1 -List
pwsh ./trace-viewer.ps1 TestClassName-trace
```

### Parallel Execution
```bash
# Use optimized test settings (4 concurrent threads)
dotnet test --settings testsettings.runsettings
```

## 🛠️ Developer Tools

### Playwright Codegen (Enhanced)
```bash
# Basic usage
pwsh ./codegen-unix.ps1

# With specific URL and browser
pwsh ./codegen-unix.ps1 -Url https://example.com -Browser firefox

# Mobile testing
pwsh ./codegen-unix.ps1 -Device "iPhone 12"

# With authentication state
pwsh ./codegen-unix.ps1 -LoadStorage auth.json -SaveStorage new-auth.json

# Show help for all options
pwsh ./codegen-unix.ps1 -Help
```

### Trace Viewer (New)
```bash
# List available traces
pwsh ./trace-viewer.ps1 -List

# Open specific trace
pwsh ./trace-viewer.ps1 BaseUrlLaunchTest-trace

# Show help
pwsh ./trace-viewer.ps1 -Help
```

### Test Development Workflow
1. **Generate initial test**: Use `pwsh ./codegen-unix.ps1` to record actions
2. **Transform to test**: Convert codegen output to `BaseUiTest` pattern
3. **Add assertions**: Use Page Object Model and proper assertions
4. **Run locally**: Use `pwsh ./run-allure-tests-unix.ps1` for full workflow
5. **Debug with traces**: Use `pwsh ./trace-viewer.ps1` for visual debugging
6. **Review reports**: Check results and screenshots in Allure

## 📝 Writing Tests

### API Test Example (Analytics)
```csharp
public class AnalyticsTest : AnalyticsBase
{
    public AnalyticsTest(ITestOutputHelper output) : base(output) { }

    [Fact]
    public async Task GetAnalytics_GetRawAsync()
    {
        // Arrange
        var caseLogger = TestLogger.Logger.ForContext("TestMethod", nameof(GetAnalytics_GetRawAsync));
        caseLogger.Information("Sending GET request to /api/analytics");

        // Act - Using GetRawAsync for JSON manipulation
        var analyticsData = await Client.GetRawAsync("/api/analytics");
        AllureHelper.AttachString("Response Body", analyticsData.Content, "application/json", ".json");

        // Deserialize and work with JSON
        JObject analyticsJson = JsonHelper.Deserialize<JObject>(analyticsData.Content);
        JToken topProducts = analyticsJson["topProducts"];
        var topProductsList = topProducts.ToObject<List<TopProduct>>();

        // Assert
        Assert.NotNull(topProductsList);
        Assert.NotEmpty(topProductsList);
        
        var topProduct = topProductsList.OrderByDescending(p => p.Quantity).First();
        caseLogger.Information($"Top Product: {topProduct.Name} (Quantity: {topProduct.Quantity})");
    }

    [Fact]
    public async Task GetAnalytics_GetTypedAsync()
    {
        // Arrange
        var caseLogger = TestLogger.Logger.ForContext("TestMethod", nameof(GetAnalytics_GetTypedAsync));

        // Act - Using strongly typed response
        var analyticsData = await Client.GetAsync<AnalyticsResponse>("/api/analytics");
        AllureHelper.AttachString("Response Body", JsonHelper.Serialize(analyticsData), "application/json", ".json");

        // Assert
        Assert.NotNull(analyticsData);
        Assert.NotEmpty(analyticsData.TopProducts);

        // LINQ usage with strongly typed model
        var topProduct = analyticsData.TopProducts.OrderByDescending(p => p.Quantity).First();
        caseLogger.Information($"Top Product: {topProduct.Name} (Quantity: {topProduct.Quantity})");
    }
}
```

### UI Test Example (Modern Page Objects)
```csharp
public class LoginToProductPage : BaseUiTest
{
    public LoginToProductPage(ITestOutputHelper output) : base(output) { }

    [Fact]
    public async Task LoginToProduct()
    {
        // Arrange - Using ILocator-based page objects
        var loginPage = new LoginPage(Page, _settings, TestLogger.Logger);
        var productPage = new UserPage(Page, _settings, TestLogger.Logger);

        // Act
        await loginPage.Login("user", "user");
        await Expect(Page.Locator("h1")).ToHaveTextAsync("Welcome, user");

        var productList = await productPage.ReturnProducts();
        
        // Assert
        Assert.Contains("Acer Predator Helios 300", productList.Keys);
    }

    [Theory]
    [InlineData(BrowserList.Edge)]
    [InlineData(BrowserList.Firefox)]
    [InlineData(BrowserList.Webkit)]
    public async Task Should_Launch_BaseUrl(BrowserList browserType)
    {
        // Arrange - Cross-browser testing with tracing
        var caseLogger = TestLogger.Logger.ForContext("Browser", browserType)
                                         .ForContext("TestMethod", nameof(Should_Launch_BaseUrl));
        
        // Launch browser with tracing enabled
        await LaunchBrowserAsync(browserType, recordVideo: true, enableTracing: true);
        var webElements = new WebElementsPage(Page, _settings, caseLogger);

        // Act
        await webElements.GoToAsync("/webelements");
        await TakeScreenshotAsync($"After navigating to : {Page.Url}");
        await webElements.EnterTextInputAsync("my first text");
        await TakeScreenshotAsync("After Input");

        // Assert - using BasePage assertion methods
        await webElements.AssertOutputContains(".web-element:has(#text-input) .web-element-output", "my first text");
        webElements.AssertUrlContains(_settings.BaseUrl);
    }
}
```

### Database Test Example (Updated Entities)
```csharp
[Fact]
public async Task Should_Save_User_To_Database()
{
    // Arrange - Using fake test data
    var user = DatabaseTestHelper.CreateFakeUser();

    // Act
    var savedUser = DatabaseTestHelper.GetUserByEmail(user.Email);

    // Assert
    Assert.NotNull(savedUser);
    Assert.Equal(user.Name, savedUser.Name);
    Assert.Equal(user.Email, savedUser.Email);
}

[Fact]
public async Task Should_Create_Multiple_Fake_Products()
{
    // Arrange - Create multiple ProductEntity instances with varied fake data
    var products = new List<ProductEntity>();
    for (int i = 0; i < 5; i++)
    {
        products.Add(DatabaseTestHelper.CreateFakeProduct());
    }

    // Act
    var activeProducts = DatabaseTestHelper.GetActiveProducts();

    // Assert
    Assert.True(activeProducts.Count >= 5);
    Assert.All(activeProducts, p => Assert.True(p.Price > 0));
}
```

## 📊 Report Management

### Quick Start with Reports
```bash
# Windows
.\run-allure-tests.ps1

# Unix/Mac/Linux
pwsh ./run-allure-tests-unix.ps1

# This will:
# 1. Clean old results while preserving history
# 2. Run smoke tests (filter Category=Smoke)
# 3. Generate fresh Allure report
# 4. Start web server on http://localhost:5000
```

### Report Management Scripts
```powershell
# Windows
.\reports\scripts\archive-reports.ps1 -BuildNumber "v1.2.3" -Branch "main"
.\reports\scripts\clean-reports.ps1
.\reports\scripts\list-archives.ps1 -Detailed
.\reports\scripts\open-report.ps1

# Unix/Mac/Linux
pwsh ./reports/scripts/archive-reports-unix.ps1 -BuildNumber "v1.2.3" -Branch "main"
pwsh ./reports/scripts/clean-reports-unix.ps1
pwsh ./reports/scripts/list-archives-unix.ps1 -Detailed
pwsh ./reports/scripts/open-report-unix.ps1
```

### Advanced Allure Features
- **Custom Categories**: Automatic classification of test failures
  - Infrastructure problems (driver/browser issues)
  - Product defects (application failures)  
  - Test defects (test code issues)  
  - Outdated tests (missing dependencies)
- **History Preservation**: Trends and history maintained across runs
- **Rich Attachments**: Screenshots, logs, API responses, and traces
- **Environment Reporting**: Browser, platform, and framework metadata
- **Parallel Execution**: Optimized for 4 concurrent threads

## 🎭 Modern Page Object Model

### Dual Approach Support

The framework supports two modern Page Object Model approaches:

#### 1. Selector-Based with BasePage Integration (Recommended for simple pages)
```csharp
public class WebElementsPage : BasePage
{
    // Selectors as constants for maintainability
    private const string TextInputSelector = "#text-input";
    private const string TextInputOutputSelector = ".web-element:has(#text-input) .web-element-output";

    public WebElementsPage(IPage page, PlaywrightSettings settings, ILogger logger) 
        : base(page, settings, logger) { }

    // Methods use BasePage for consistent error handling
    public async Task EnterTextInputAsync(string text) =>
        await FillAsync(TextInputSelector, text);

    public async Task<string> GetTextInputOutputAsync() =>
        await InnerTextAsync(TextInputOutputSelector);
}
```

#### 2. ILocator-Based with BasePageILocator (Recommended for complex pages)
```csharp
public class LoginPage : BasePageILocator
{
    // ILocators for modern Playwright patterns
    public ILocator UsernameInput => Page.Locator("#username");
    public ILocator PasswordInput => Page.Locator("#password");
    public ILocator LoginButton => Page.GetByRole(AriaRole.Button, new() { Name = "Log in" });

    public LoginPage(IPage page, PlaywrightSettings settings, ILogger logger) 
        : base(page, settings, logger) { }

    public async Task Login(string username, string password)
    {
        await UsernameInput.FillAsync(username);
        await PasswordInput.FillAsync(password);
        await LoginButton.ClickAsync();
    }
}
```

## 🔍 Key Architectural Changes

### Recent Updates (2024)

#### API Model Refactoring
- **AnalyticsClient**: New client for analytics operations (replaces UserApiClient)
- **Enhanced API Models**: 
  - `AnalyticsResponse` - Complex analytics data structure
  - `MergedProduct` - Product merger operations
  - `TopProduct` - Product with quantity tracking (extends base Product)
  - `Product` - New base product model with common fields
- **Inheritance-Based Models**: ProductRequest and TopProduct inherit from Product base class

#### Database Layer Improvements
- **ProductEntity**: Renamed from Product to avoid naming conflicts with API models
- **Repository Updates**: All repositories updated to use ProductEntity
- **Test Data Updates**: Faker and helper classes updated for new entity structure

#### Tracing Integration
- **Built-in Tracing**: Playwright tracing integrated into PlaywrightSetup
- **Configurable Tracing**: Control via appsettings.json or per-test parameters
- **Trace Viewer**: New PowerShell script for easy trace viewing
- **Automatic Management**: Traces saved automatically with test execution

#### Enhanced Developer Tools
- **Cross-Platform Codegen**: Enhanced codegen-unix.ps1 with advanced options
- **Trace Viewer**: PowerShell-based trace viewer with file management
- **Improved Scripts**: Better error handling and cross-platform support

## 🔄 CI/CD Integration

### GitHub Actions Example
```yaml
name: Test Automation

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v4
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '9.0'
    
    - name: Install PowerShell
      run: |
        sudo apt-get update
        sudo apt-get install -y powershell
    
    - name: Install Playwright
      run: pwsh bin/Debug/net9.0/playwright.ps1 install --with-deps
    
    - name: Run Tests with Allure
      run: |
        pwsh ./run-allure-tests-unix.ps1
        pwsh ./reports/scripts/archive-reports-unix.ps1 -BuildNumber "${{ github.run_number }}" -Branch "${{ github.ref_name }}"
    
    - name: Upload Allure Results
      uses: actions/upload-artifact@v4
      if: always()
      with:
        name: allure-results-${{ github.run_number }}
        path: reports/allure-results/
    
    - name: Upload Traces
      uses: actions/upload-artifact@v4
      if: always()
      with:
        name: playwright-traces-${{ github.run_number }}
        path: bin/Debug/net9.0/Traces/
```

## 🧪 Test Data Generation

### Enhanced Fake Data Support
```csharp
// Complete fake data for ProductEntity
var product = TestDataFaker.CreateFakeProduct();

// Mix of fake and specific data for API models
var productRequest = new ProductRequestBuilder()
    .WithFakeId()           // Random ID
    .WithFakeName()         // Random product name  
    .WithCategory("Laptops") // Specific category
    .WithFakePrice(50, 200) // Random price in range
    .Build();

// Direct faker usage
var email = TestDataFaker.FakeEmail();
var category = TestDataFaker.FakeCategory();
var price = TestDataFaker.FakePrice(10, 500);

// Database entities with fake data
var user = DatabaseTestHelper.CreateFakeUser();
var product = DatabaseTestHelper.CreateFakeProduct();
```

## 🔧 Troubleshooting

### Common Issues

**Trace Files Not Generated**
```bash
# Enable tracing in config
"Playwright": { "EnableTracing": true }

# Or per test
await LaunchBrowserAsync(BrowserList.Chrome, enableTracing: true);

# Check trace directory
pwsh ./trace-viewer.ps1 -List
```

**Cross-Platform Script Issues**
```bash
# Ensure PowerShell is installed
pwsh --version

# Use Unix scripts on Mac/Linux
pwsh ./run-allure-tests-unix.ps1
pwsh ./codegen-unix.ps1
```

**API Model Conflicts**
```csharp
// Use fully qualified names if needed
using ApiProduct = atf.API.Models.Product;
using DataProduct = atf.Data.Models.ProductEntity;
```

**Port Already in Use**
```powershell
# Scripts automatically find available ports
pwsh ./reports/scripts/open-report-unix.ps1 -Port 3000
```

## 🤝 Contributing

### Code Style
- Follow **C# naming conventions**
- Use **async/await** for all I/O operations
- Implement **proper error handling** with meaningful messages
- Add **XML documentation** for public APIs
- Maintain **cross-platform compatibility** for scripts

### Testing Guidelines
- **One assertion per test** when possible
- Use **descriptive test names** that explain the scenario
- Follow **Arrange-Act-Assert** pattern
- **Clean up resources** in test teardown
- **Use appropriate Page Object approach** (selector-based vs ILocator)

### Pull Request Process
1. Create feature branch from `main`
2. Write tests for new functionality
3. Ensure all tests pass locally with tracing enabled
4. Update documentation if needed
5. Submit PR with descriptive title and details

## 📄 License

This project is licensed under the [MIT License](LICENSE).

## 🆘 Support

- **Issues**: Report bugs and feature requests in [GitHub Issues](../../issues)
- **Documentation**: Additional docs in `/Documentation` folder
- **Examples**: Sample tests in `/Tests` directory
- **Tracing**: Use `pwsh ./trace-viewer.ps1` for visual debugging

---

**Happy Testing!** 🎯