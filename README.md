# .NET Test Automation Framework

*Sponsored by coffee ☕, Claude 🤖, and bench 💺*

A comprehensive test automation framework built with **.NET 9** for API, UI, and database testing. Designed with clean architecture principles and follows industry best practices for maintainable and scalable test automation.

## 🚀 Features

- **Multi-Layer Testing**: API, UI, and Database test support
- **Cross-Browser Testing**: Playwright integration with Chrome, Firefox, Safari, and Edge
- **API Testing**: RestSharp-based HTTP client with fluent builders
- **Database Testing**: Entity Framework Core with SQL Server and In-Memory providers
- **Rich Reporting**: Allure integration with screenshots and detailed logs
- **Structured Logging**: Serilog with configurable outputs and test context
- **Configuration Management**: Environment-specific settings with `appsettings.json`
- **Clean Architecture**: Layered design with clear separation of concerns
- **No DI Container**: Manual dependency wiring for simplicity and control

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
| **Test Data** | AutoFixture | Test data generation |

## 📁 Project Structure

```
├── API/                           # API Testing Layer
│   ├── Builders/                  # Request builders (Fluent API)
│   │   ├── HttpRequestBuilder.cs
│   │   └── ProductRequestBuilder.cs
│   ├── Clients/                   # HTTP client wrappers
│   │   ├── BaseClient.cs
│   │   └── ProductApiClient.cs
│   ├── Fixtures/                  # xUnit test fixtures
│   │   ├── ApiTestFixture.cs
│   │   └── DefaultApiTestFixture.cs
│   └── Models/                    # API request/response models
│       └── ProductRequest.cs
│
├── Core/                          # Cross-cutting Concerns
│   ├── Config/                    # Configuration management
│   │   ├── ConfigManager.cs
│   │   └── IConfigManager.cs
│   ├── Enums/                     # Framework enumerations
│   │   └── BrowserList.cs
│   ├── Logging/                   # Logging utilities
│   │   └── CustomLogger.cs
│   ├── Models/                    # Framework models
│   │   ├── BaseModel.cs
│   │   └── PlaywrightSettings.cs
│   └── Utils/                     # Helper utilities
│       └── JsonHelper.cs
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
│   ├── Helpers/                   # Test helper utilities
│   │   ├── AllureHelper.cs
│   │   └── DataBaseTestHelper.cs
│   ├── Tests.API/                 # API test implementations
│   │   ├── BaseApiTest.cs
│   │   └── ProductApiTests.cs
│   ├── Tests.Mocks/               # Mock and test data providers
│   │   ├── BaseMockProvider.cs
│   │   └── ProductServiceTest.cs
│   └── Tests.UI/                  # UI test implementations
│       ├── BaseUiTest.cs
│       └── BaseUrlLaunchTest.cs
│
├── UI/                            # UI Testing Layer
│   ├── Locators/                  # Element selectors
│   │   └── WebElementsSelectors.cs
│   ├── Pages/                     # Page Object Models
│   │   ├── BasePage.cs
│   │   └── WebElementsPage.cs
│   └── PlaywrightSetup/           # Browser initialization
│       └── PlaywrightSetup.cs
│
├── Documentation/                 # Project documentation
└── allure-results/               # Test execution reports
```

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
    "BaseApiHost": "http://localhost:5001",
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
    "BaseUrl": "https://jsonplaceholder.typicode.com",
    "HttpBinUrl": "https://httpbin.org"
  }
}
```

### Environment Overrides
- Create `appsettings.Development.json` for local settings
- Use environment variables: `Playwright__BrowserType=Chrome`

## 🧪 Running Tests

### All Tests
```bash
dotnet test
```

### Specific Test Categories
```bash
# API Tests only
dotnet test --filter Category=API

# UI Tests only
dotnet test --filter Category=UI

# Database Tests only
dotnet test --filter Category=Database
```

### With Allure Reporting
```bash
dotnet test --logger:allure
allure serve allure-results
```

### Headless Mode
```bash
# Override browser settings
Playwright__Headless=true dotnet test
```

## 📝 Writing Tests

### API Test Example
```csharp
public class ProductApiTests : BaseApiTest<DefaultApiTestFixture>
{
    public ProductApiTests(DefaultApiTestFixture fixture, ITestOutputHelper output) 
        : base(fixture, output) { }

    [Fact]
    public async Task Should_Create_Product_Successfully()
    {
        // Arrange
        var product = new ProductRequestBuilder()
            .WithName("Test Product")
            .WithPrice(99.99m)
            .Build();

        // Act
        var result = await Client.CreateProductAsync(product);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Product", result.Name);
    }
}
```

### UI Test Example
```csharp
public class LoginTests : BaseUiTest
{
    public LoginTests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public async Task Should_Login_With_Valid_Credentials()
    {
        // Arrange
        var loginPage = new LoginPage(Page);
        
        // Act
        await loginPage.NavigateToAsync();
        await loginPage.LoginAsync("testuser", "password");
        
        // Assert
        await Expect(Page.Locator(".welcome-message")).ToBeVisibleAsync();
        await TakeScreenshot("successful-login");
    }
}
```

### Database Test Example
```csharp
[Fact]
public async Task Should_Save_User_To_Database()
{
    // Arrange
    using var context = DbContextFactory.CreateInMemoryContext();
    var repository = new UserRepository(context);
    var user = new User { Name = "Test User", Email = "test@example.com" };

    // Act
    await repository.AddAsync(user);
    var savedUser = await repository.GetByEmailAsync("test@example.com");

    // Assert
    Assert.NotNull(savedUser);
    Assert.Equal("Test User", savedUser.Name);
}
```

## 📊 Reporting

### Allure Reports
- **Rich HTML reports** with test history and trends
- **Screenshot attachments** for UI test failures
- **Request/Response logs** for API tests
- **Environment information** and test metadata

### Accessing Reports
1. Run tests: `dotnet test --logger:allure`
2. Generate report: `allure serve allure-results`
3. View in browser at `http://localhost:port`

### Custom Attachments
```csharp
// Screenshot
await TakeScreenshot("test-state");

// JSON data
AllureHelper.AttachString("API Response", responseJson);

// Custom files
AllureApi.AddAttachment("log-file", "text/plain", logContent);
```

## 🔍 Key Features

### Configuration Management
- **IConfigManager interface** for dependency injection
- **Environment-specific** configuration support
- **Type-safe configuration** with strongly-typed settings classes

### Browser Management
- **Multi-browser support**: Chrome, Firefox, Safari, Edge
- **Configurable viewports** and timeouts
- **Automatic screenshot capture** on test failures

### API Testing
- **Fluent request builders** for clean test data setup
- **Automatic retries** and error handling
- **Request/Response logging** for debugging

### Database Testing
- **In-memory database** for fast unit tests
- **Real database** for integration tests
- **Automatic cleanup** and test isolation

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