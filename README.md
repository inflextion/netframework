# .NET Test Automation Framework

*Sponsored by coffee ☕, AI 🤖, and bench 💺*

A comprehensive test automation framework built with **.NET 9** for API, UI, and database testing. Designed with clean architecture principles and follows industry best practices for maintainable and scalable test automation.

## 🚀 Features

- **Multi-Layer Testing**: API, UI, and Database test support
- **Cross-Browser Testing**: Playwright integration with Chrome, Firefox, Safari, and Edge
- **Modern Page Object Model**: Selector-based approach with BasePage common methods and consistent error handling
- **API Testing**: RestSharp-based HTTP client with fluent builders and factory pattern
- **Database Testing**: Entity Framework Core with SQL Server and In-Memory providers
- **Advanced Reporting**: Comprehensive Allure system with automated archiving and history preservation
- **Report Management**: Professional scripts for archiving, cleaning, and serving reports
- **Thread-Safe Logging**: TestLogger with per-class file logging and console output
- **Static Configuration**: Simple ConfigManager for direct configuration access
- **Fake Test Data**: Bogus library integration for realistic test data generation
- **Developer Tooling**: Playwright codegen integration and test generation workflow
- **CI/CD Ready**: GitHub Actions and Azure DevOps pipeline examples
- **Clean Architecture**: Layered design with clear separation of concerns
- **Simplified Dependencies**: Static configuration management without dependency injection

## 🏗️ Architecture

### Design Patterns
- **Page Object Model** for UI test organization
- **Repository Pattern** for data access abstraction
- **Builder Pattern** for test data and request construction
- **Factory Pattern** for database and browser initialization
- **Template Method** for base test classes

### SOLID Principles
- ✅ **Single Responsibility**: Each class has a focused purpose
- ✅ **Open/Closed**: Extensible base classes without modification
- ✅ **Liskov Substitution**: Proper inheritance hierarchies
- ✅ **Interface Segregation**: Focused interfaces where needed
- ✅ **Dependency Inversion**: Abstractions over concrete implementations

## 🛠️ Technologies

| Category | Technology | Purpose |
|----------|------------|---------|
| **Framework** | .NET 9 | Core platform |
| **Testing** | xUnit | Test runner and assertions |
| **UI Automation** | Playwright | Cross-browser automation |
| **API Testing** | RestSharp | HTTP client and requests |
| **Database** | Entity Framework Core | ORM and database testing |
| **Logging** | Serilog | Structured logging |
| **Reporting** | Allure | Test reporting and analytics |
| **Configuration** | Microsoft.Extensions.Configuration | Settings management |
| **Test Data** | Bogus | Fake test data generation |

## 📁 Project Structure

```
├── API/                           # API Testing Layer
│   ├── Builders/                  # Request builders (Fluent API)
│   │   └── ProductRequestBuilder.cs
│   ├── Clients/                   # HTTP client wrappers
│   │   ├── ApiClientFactory.cs    # Factory for creating API clients
│   │   ├── BaseClient.cs          # Base HTTP client with common operations
│   │   └── ProductApiClient.cs    # Product-specific API client
│   ├── Fixtures/                  # xUnit test fixtures
│   │   ├── ApiTestFixture.cs
│   │   └── DefaultApiTestFixture.cs
│   └── Models/                    # API request/response models
│       └── ProductRequest.cs
│
├── Core/                          # Cross-cutting Concerns
│   ├── Config/                    # Configuration management
│   │   └── ConfigManager.cs       # Static configuration manager
│   ├── Enums/                     # Framework enumerations
│   │   └── BrowserList.cs
│   ├── Logging/                   # Logging utilities
│   │   └── TestLogger.cs          # Thread-safe test logging
│   ├── Models/                    # Framework models
│   │   ├── BaseModel.cs
│   │   └── PlaywrightSettings.cs
│   └── Utils/                     # Helper utilities
│       ├── JsonHelper.cs
│       └── TestDataFaker.cs       # Fake test data generation
│
├── Data/                          # Data Access Layer
│   ├── DatabaseContext/           # Database context management
│   │   ├── DbContextFactory.cs
│   │   └── TestDbContext.cs
│   ├── Models/                    # Entity models
│   │   ├── Product.cs
│   │   └── User.cs
│   └── Repositories/              # Data access repositories
│       ├── IProductRepository.cs
│       ├── IUserRepository.cs
│       ├── ProductRepository.cs
│       └── UserRepository.cs
│
├── Tests/                         # Test Implementation
│   ├── Base/                      # Base test classes
│   │   ├── BaseApiTest.cs         # Generic API test base class
│   │   └── ProductApiTestBase.cs  # Product-specific API test base
│   ├── Helpers/                   # Test helper utilities
│   │   ├── AllureHelper.cs
│   │   └── DataBaseTestHelper.cs
│   ├── Tests.API/                 # API test implementations
│   │   └── ProductApiTests.cs     # Product API test cases
│   ├── Tests.Mocks/               # Mock and test data providers
│   │   ├── BaseMockProvider.cs
│   │   └── ProductServiceTest.cs
│   └── Tests.UI/                  # UI test implementations
│       ├── AdvancedPageTests.cs
│       ├── BaseUiTest.cs
│       └── BaseUrlLaunchTest.cs
│
├── UI/                            # UI Testing Layer
│   ├── Pages/                     # Page Object Models with selector-based approach
│   │   ├── BasePage.cs            # Base page with common functionality and error handling
│   │   ├── WebElementsPage.cs     # Page with selector constants and BasePage methods
│   │   └── AdvancedWebElementsPage.cs # Advanced page interactions using BasePage methods
│   └── PlaywrightSetup/           # Browser initialization
│       └── PlaywrightSetup.cs
│
├── Documentation/                 # Project documentation
│   ├── from-java-cheatsheet.md   # Java to .NET transition guide
│   ├── playwright-selectors-cheatsheet.md # Comprehensive Playwright selectors reference
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
├── run-allure-tests.ps1         # Main test execution script (Windows)
├── run-allure-tests-unix.ps1    # Main test execution script (Unix/Mac)
├── testsettings.runsettings     # XUnit parallel execution settings
└── codegen.ps1                  # Playwright codegen tool launcher
```

## 🖥️ Cross-Platform Support

The framework provides separate execution scripts for Windows and Unix-based systems (macOS, Linux):

### Windows Scripts
- `run-allure-tests.ps1` - Main test execution
- `reports/scripts/*.ps1` - Report management scripts

### Unix/Mac/Linux Scripts  
- `run-allure-tests-unix.ps1` - Main test execution
- `reports/scripts/*-unix.ps1` - Report management scripts

The Unix scripts use forward slashes for paths and Unix-specific commands for optimal compatibility.

## ⚙️ Setup

### Prerequisites
- **.NET 9 SDK** or later
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
    "DefaultTimeoutMs": 30000,
    "ViewportWidth": 1280,
    "ViewportHeight": 800
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TestDb_Tests;Trusted_Connection=true;"
  },
  "ApiClient": {
    "TimeoutMs": 30000,
    "RetryAttempts": 3
  },
  "TestApiSettings": {
    "BaseUrl": "http://localhost:5001",
  }
}
```

### Environment Overrides
- Create `appsettings.Development.json` for local settings
- Use environment variables: `Playwright__BrowserType=Chrome`

## 🧪 Running Tests

### Quick Test Execution
```bash
# Windows
.\run-allure-tests.ps1

# Unix/Mac/Linux
.\run-allure-tests-unix.ps1

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

### Parallel Execution
```bash
# Use optimized test settings
dotnet test --settings testsettings.runsettings
```

### Manual Allure Reporting
```bash
# Generate report only (after test run)
allure generate reports/allure-results -o reports/allure-report --clean

# Serve existing report
# Windows
.\reports\scripts\open-report.ps1 -Port 8080

# Unix/Mac/Linux  
.\reports\scripts\open-report-unix.ps1 -Port 8080
```

## 📝 Writing Tests

### API Test Example
```csharp
public class ProductApiTests : ProductApiTestBase
{
    public ProductApiTests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public async Task Should_Create_Product_Successfully()
    {
        // Arrange - Using inherited TestLogger with additional context
        var caseLogger = TestLogger.Logger.ForContext("TestMethod", nameof(Should_Create_Product_Successfully));
        
        var product = new ProductRequestBuilder()
            .WithFakeData()  // Generates realistic fake data
            .Build();

        caseLogger.Information("Creating product: {@Product}", product);

        // Act
        var result = await Client.CreateProductAsync(product);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.Name, result.Name);
        Assert.True(result.Price > 0);
        
        caseLogger.Information("Product created successfully with ID: {Id}", result.Id);
    }

    [Fact]
    public async Task Should_Create_Product_With_Mixed_Data()
    {
        // Arrange - Mix of fake and specific data
        var product = new ProductRequestBuilder()
            .WithFakeId()           // Random ID
            .WithFakeName()         // Random product name
            .WithCategory("Laptops") // Specific category
            .WithFakePrice(100, 500) // Random price in range
            .Build();

        // Act
        var result = await Client.CreateProductAsync(product);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Laptops", result.Category);
        Assert.True(result.Price >= 100 && result.Price <= 500);
    }
}
```

### UI Test Example with Selector-based Page Objects
```csharp
public class WebElementsTests : BaseUiTest
{
    public WebElementsTests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public async Task Should_Enter_Text_And_Verify_Output()
    {
        // Arrange - Using inherited TestLogger with additional context
        var caseLogger = TestLogger.Logger.ForContext("TestMethod", nameof(Should_Enter_Text_And_Verify_Output));
        var webElementsPage = new WebElementsPage(Page, _settings, caseLogger);
        
        // Act
        await webElementsPage.GoToAsync("/webelements");
        await webElementsPage.EnterTextInputAsync("test input");
        
        // Assert - using BasePage assertion methods
        await webElementsPage.AssertOutputContains(".web-element:has(#text-input) .web-element-output", "test input");
        await TakeScreenshotAsync("text-input-verification");
    }

    [Theory]
    [InlineData(BrowserList.Edge)]
    [InlineData(BrowserList.Firefox)]
    [InlineData(BrowserList.Webkit)]
    public async Task Should_Handle_Cross_Browser_Testing(BrowserList browserType)
    {
        // Arrange - Inherited TestLogger with browser context
        var caseLogger = TestLogger.Logger.ForContext("TestMethod", $"{nameof(Should_Handle_Cross_Browser_Testing)}_{browserType}");
        await LaunchBrowserAsync(browserType);
        var webElementsPage = new WebElementsPage(Page, _settings, caseLogger.ForContext("Browser", browserType));
        
        // Act & Assert
        await webElementsPage.GoToAsync("/webelements");
        await webElementsPage.EnterTextInputAsync("cross-browser test");
        
        // Verify output using BasePage methods
        var output = await webElementsPage.GetTextInputOutputAsync();
        Assert.Contains("cross-browser test", output);
    }
}
```

### Database Test Example
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
    // Arrange - Create multiple products with varied fake data
    var products = new List<Product>();
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
.\run-allure-tests-unix.ps1

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
.\reports\scripts\archive-reports-unix.ps1 -BuildNumber "v1.2.3" -Branch "main"
.\reports\scripts\clean-reports-unix.ps1
.\reports\scripts\list-archives-unix.ps1 -Detailed
.\reports\scripts\open-report-unix.ps1
```

### Advanced Allure Features
- **Custom Categories**: Automatic classification of test failures
  - Infrastructure problems (driver/browser issues)
  - Product defects (application failures)  
  - Test defects (test code issues)  
  - Outdated tests (missing dependencies)
- **History Preservation**: Trends and history maintained across runs
- **Rich Attachments**: Screenshots, logs, and API responses
- **Environment Reporting**: Browser, platform, and framework metadata
- **Parallel Execution**: Optimized for 4 concurrent threads

### Accessing Reports
```bash
# Quick access (recommended)
# Windows
.\run-allure-tests.ps1
# Unix/Mac/Linux
.\run-allure-tests-unix.ps1

# Manual generation
dotnet test --logger:allure
allure generate reports/allure-results -o reports/allure-report --clean
allure open reports/allure-report

# Using scripts
# Windows
.\reports\scripts\open-report.ps1 -Port 3000
# Unix/Mac/Linux
.\reports\scripts\open-report-unix.ps1 -Port 3000
```

### Custom Attachments
```csharp
// Screenshot with automatic failure capture
await TakeScreenshotAsync("test-state");

// JSON data attachment
AllureHelper.AttachString("API Response", responseJson);
AllureHelper.WriteAllureEnvironmentProperties();

// Custom files and logs
AllureApi.AddAttachment("log-file", "text/plain", logContent);
```

## 🔄 CI/CD Integration

### GitHub Actions Example
```yaml
name: Test Automation

on: [push, pull_request]

jobs:
  test:
    runs-on: windows-latest
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '9.0'
    
    - name: Install Playwright
      run: pwsh bin/Debug/net9.0/playwright.ps1 install
    
    - name: Run Tests with Allure
      run: |
        .\run-allure-tests.ps1
        .\reports\scripts\archive-reports.ps1 -BuildNumber "${{ github.run_number }}" -Branch "${{ github.ref_name }}"
    
    - name: Upload Allure Results
      uses: actions/upload-artifact@v3
      if: always()
      with:
        name: allure-results-${{ github.run_number }}
        path: reports/allure-results/
    
    - name: Upload Archived Reports
      uses: actions/upload-artifact@v3
      if: always()
      with:
        name: archived-reports-${{ github.run_number }}
        path: reports/archive/
```

### Azure DevOps Pipeline
```yaml
trigger:
- main

pool:
  vmImage: 'windows-latest'

steps:
- task: UseDotNet@2
  inputs:
    version: '9.0.x'

- script: |
    pwsh bin/Debug/net9.0/playwright.ps1 install
  displayName: 'Install Playwright Browsers'

- script: |
    .\run-allure-tests.ps1
    .\reports\scripts\archive-reports.ps1 -BuildNumber "$(Build.BuildNumber)" -Branch "$(Build.SourceBranchName)"
  displayName: 'Run Tests and Archive Reports'

- task: PublishTestResults@2
  inputs:
    testResultsFormat: 'VSTest'
    testResultsFiles: '**/*.trx'

- task: PublishBuildArtifacts@1
  inputs:
    pathToPublish: 'reports/allure-results'
    artifactName: 'allure-results'
```

### Docker Support
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:9.0

WORKDIR /app
COPY . .

RUN dotnet restore
RUN dotnet build

# Install Playwright dependencies
RUN pwsh bin/Debug/net9.0/playwright.ps1 install-deps
RUN pwsh bin/Debug/net9.0/playwright.ps1 install

CMD ["./run-allure-tests.ps1"]
```

## 🛠️ Developer Tools

### Playwright Codegen
```bash
# Launch Playwright code generator
.\codegen.ps1

# With specific URL and browser
.\codegen.ps1 -Url https://example.com -Browser firefox

# Mobile testing
.\codegen.ps1 -Device "iPhone 12"

# With authentication state
.\codegen.ps1 -LoadStorage auth.json -SaveStorage new-auth.json
```

### Test Development Workflow
1. **Generate initial test**: Use `.\codegen.ps1` to record actions
2. **Transform to test**: Convert codegen output to `BaseUiTest` pattern
3. **Add assertions**: Use Page Object Model and proper assertions
4. **Run locally**: Use `.\run-allure-tests.ps1` for full workflow
5. **Review reports**: Check results and screenshots in Allure

## 🔧 Troubleshooting

### Common Issues

**Port Already in Use**
```powershell
# The open-report script automatically finds available ports
.\reports\scripts\open-report.ps1 -Port 3000
```

**Allure Command Not Found**
```bash
# Install Allure via npm (global)
npm install -g allure-commandline

# Or use the bundled commands in run-allure-tests.ps1
```

**History Not Preserved**  
History is automatically managed by `run-allure-tests.ps1`. For manual operations:
```bash
# Backup history before cleaning
Copy-Item reports/allure-report/history reports/history_backup -Recurse
# Restore after test run
Copy-Item reports/history_backup/* reports/allure-results/history/
```

**Playwright Browser Issues**
```bash
# Reinstall browsers
pwsh bin/Debug/net9.0/playwright.ps1 install --force

# Check browser installation
pwsh bin/Debug/net9.0/playwright.ps1 install --dry-run
```

**Test Execution Timeouts**
```bash
# Increase default timeouts in appsettings.json
"Playwright": {
  "DefaultTimeoutMs": 60000  // 60 seconds
}

# Or via environment variables
$env:Playwright__DefaultTimeoutMs="60000"
```

## 🎭 Modern Page Object Model

### Selector-Based Approach with BasePage Integration
The framework uses a simplified selector-based approach that leverages BasePage common methods for consistent error handling and logging:

```csharp
public class WebElementsPage : BasePage
{
    // Selectors as constants for maintainability
    private const string TextInputSelector = "#text-input";
    private const string TextInputOutputSelector = ".web-element:has(#text-input) .web-element-output";
    private const string CounterIncrementButtonSelector = ".web-element-counter button:has-text(\"+\")";
    private const string DropdownSelector = "#dropdown";

    public WebElementsPage(IPage page, PlaywrightSettings settings, ILogger logger) 
        : base(page, settings, logger)
    {
        // No locator initialization needed - selectors used directly
    }

    // Methods use BasePage methods for consistent error handling and logging
    public async Task EnterTextInputAsync(string text) =>
        await FillAsync(TextInputSelector, text);

    public async Task<string> GetTextInputOutputAsync() =>
        await InnerTextAsync(TextInputOutputSelector);

    public async Task ClickCounterIncrementAsync() =>
        await ClickAsync(CounterIncrementButtonSelector);

    public async Task SelectDropdownAsync(string value) =>
        await SelectOptionAsync(DropdownSelector, value);
}
```

### Advanced Selector Patterns
```csharp
public class AdvancedWebElementsPage : BasePage
{
    // Complex selectors for nested elements
    private const string CounterValueSelector = ".advanced-counter-section .counter-value";
    private const string CheckboxSelector = ".advanced-checkbox-section input[type='checkbox']";

    public AdvancedWebElementsPage(IPage page, PlaywrightSettings settings, ILogger logger) 
        : base(page, settings, logger)
    {
    }

    // Index-based element selection using CSS selectors
    public async Task CheckCheckboxAsync(int index) =>
        await CheckAsync($"{CheckboxSelector}:nth-child({index + 1})");

    // Parameterized selectors for dynamic content
    public async Task SelectRadioAsync(string value) =>
        await CheckAsync($".web-element-radio-input[value='{value}']");

    // Using BasePage assertion methods
    public async Task AssertTextOutputAsync(string text) =>
        await AssertOutputContains(TextInputOutputSelector, text);
}
```

## 🔍 Key Features

### Configuration Management
- **Static ConfigManager** for simple configuration access without dependency injection
- **Environment-specific** configuration support via appsettings files
- **Type-safe configuration** with strongly-typed settings classes
- **Direct access**: `ConfigManager.Get<string>("key")` and `ConfigManager.GetSection<T>("section")`

### Browser Management
- **Multi-browser support**: Chrome, Firefox, Safari, Edge
- **Configurable viewports** and timeouts
- **Automatic screenshot capture** on test failures
- **Cross-browser testing** with Theory attributes and BrowserList enum

### Page Object Model
- **Selector-based approach**: Simple CSS selector constants with BasePage method integration
- **Consistent error handling**: All actions use BasePage methods with standardized logging
- **Built-in waiting**: Automatic element waiting through Playwright's Page methods
- **Maintainable selectors**: Centralized selector constants for easy maintenance
- **Simplified architecture**: No locator initialization overhead
- **Enhanced debugging**: Detailed logging and error messages for all interactions

### API Testing
- **ApiClientFactory pattern** for centralized client configuration
- **Fluent request builders** for clean test data setup
- **Inheritance-based architecture** with BaseClient and specific API clients
- **Automatic retries** and error handling
- **Request/Response logging** for debugging

### Database Testing
- **In-memory database** for fast unit tests
- **Real database** for integration tests
- **Automatic cleanup** and test isolation

### Test Data Generation
- **Bogus library integration** for realistic fake data
- **Fluent builder patterns** with faker methods
- **Mix fake and real data** for flexible test scenarios
- **No hardcoded test values** - fresh data every test run

## 🎭 Test Data Examples

### Using Fake Data in Builders
```csharp
// Complete fake data
var product = new ProductRequestBuilder()
    .WithFakeData()
    .Build();

// Mix of fake and specific data
var product = new ProductRequestBuilder()
    .WithFakeId()           // Random ID
    .WithFakeName()         // Random product name  
    .WithCategory("Laptops") // Specific category
    .WithFakePrice(50, 200) // Random price in range
    .Build();
```

### Direct Faker Usage
```csharp
// Generate individual fake values
var email = TestDataFaker.FakeEmail();
var name = TestDataFaker.FakeName();
var price = TestDataFaker.FakePrice(10, 500);
var category = TestDataFaker.FakeCategory();

// Create complete fake entities
var user = TestDataFaker.CreateFakeUser();
var product = TestDataFaker.CreateFakeProduct();
```

### Database Test Data
```csharp
// Create fake entities in database
var user = DatabaseTestHelper.CreateFakeUser();
var product = DatabaseTestHelper.CreateFakeProduct();

// Traditional approach still available
var user = DatabaseTestHelper.CreateTestUser("Specific Name", "specific@email.com");
```

## 🤝 Contributing

### Code Style
- Follow **C# naming conventions**
- Use **async/await** for all I/O operations
- Implement **proper error handling** with meaningful messages
- Add **XML documentation** for public APIs

### Testing Guidelines
- **One assertion per test** when possible
- Use **descriptive test names** that explain the scenario
- Follow **Arrange-Act-Assert** pattern
- **Clean up resources** in test teardown

### Pull Request Process
1. Create feature branch from `main`
2. Write tests for new functionality
3. Ensure all tests pass locally
4. Update documentation if needed
5. Submit PR with descriptive title and details

## 📄 License

This project is licensed under the [MIT License](LICENSE).

## 🆘 Support

- **Issues**: Report bugs and feature requests in [GitHub Issues](../../issues)
- **Documentation**: Additional docs in `/Documentation` folder
- **Examples**: Sample tests in `/Tests` directory

---

**Happy Testing!** 🎯
