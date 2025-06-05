# .NET Playwright Automation Framework Architecture

## Project Structure

```
TestAutomationFramework/
├── src/
│   ├── TestAutomationFramework.Core/
│   │   ├── Configuration/
│   │   │   ├── ITestConfiguration.cs
│   │   │   ├── TestConfiguration.cs
│   │   │   └── ConfigurationManager.cs
│   │   ├── Constants/
│   │   │   ├── TestConstants.cs
│   │   │   └── ApiEndpoints.cs
│   │   ├── Models/
│   │   │   ├── User.cs
│   │   │   ├── ApiResponse.cs
│   │   │   └── TestData.cs
│   │   ├── Enums/
│   │   │   ├── Browser.cs
│   │   │   └── Environment.cs
│   │   ├── Extensions/
│   │   │   ├── PlaywrightExtensions.cs
│   │   │   ├── StringExtensions.cs
│   │   │   └── CollectionExtensions.cs
│   │   ├── Utilities/
│   │   │   ├── FileHelper.cs
│   │   │   ├── CsvHelper.cs
│   │   │   ├── JsonHelper.cs
│   │   │   ├── DateTimeHelper.cs
│   │   │   └── RandomDataGenerator.cs
│   │   └── Exceptions/
│   │       ├── FrameworkException.cs
│   │       └── TestDataException.cs
│   │
│   ├── TestAutomationFramework.UI/
│   │   ├── Base/
│   │   │   ├── BaseTest.cs
│   │   │   ├── BasePage.cs
│   │   │   └── BaseComponent.cs
│   │   ├── Drivers/
│   │   │   ├── IDriverManager.cs
│   │   │   ├── DriverManager.cs
│   │   │   └── BrowserFactory.cs
│   │   ├── Pages/
│   │   │   ├── LoginPage.cs
│   │   │   ├── HomePage.cs
│   │   │   └── ProductPage.cs
│   │   ├── Components/
│   │   │   ├── HeaderComponent.cs
│   │   │   ├── FooterComponent.cs
│   │   │   └── NavigationComponent.cs
│   │   ├── Locators/
│   │   │   ├── LoginPageLocators.cs
│   │   │   └── HomePageLocators.cs
│   │   └── Helpers/
│   │       ├── WaitHelper.cs
│   │       ├── ActionHelper.cs
│   │       └── ScreenshotHelper.cs
│   │
│   ├── TestAutomationFramework.API/
│   │   ├── Base/
│   │   │   ├── BaseApiTest.cs
│   │   │   └── BaseApiClient.cs
│   │   ├── Clients/
│   │   │   ├── IUserApiClient.cs
│   │   │   ├── UserApiClient.cs
│   │   │   ├── IProductApiClient.cs
│   │   │   └── ProductApiClient.cs
│   │   ├── Requests/
│   │   │   ├── CreateUserRequest.cs
│   │   │   └── UpdateUserRequest.cs
│   │   ├── Responses/
│   │   │   ├── UserResponse.cs
│   │   │   └── ProductResponse.cs
│   │   ├── Mocks/
│   │   │   ├── IApiMockServer.cs
│   │   │   └── WireMockServer.cs
│   │   └── Helpers/
│   │       ├── AuthHelper.cs
│   │       └── RequestHelper.cs
│   │
│   ├── TestAutomationFramework.Database/
│   │   ├── Base/
│   │   │   ├── BaseRepository.cs
│   │   │   └── IBaseRepository.cs
│   │   ├── Repositories/
│   │   │   ├── IUserRepository.cs
│   │   │   ├── UserRepository.cs
│   │   │   ├── IProductRepository.cs
│   │   │   └── ProductRepository.cs
│   │   ├── Context/
│   │   │   └── TestDbContext.cs
│   │   ├── Entities/
│   │   │   ├── UserEntity.cs
│   │   │   └── ProductEntity.cs
│   │   └── Migrations/
│   │
│   └── TestAutomationFramework.Reporting/
│       ├── Allure/
│       │   ├── AllureExtensions.cs
│       │   ├── AllureStepAttribute.cs
│       │   └── AllureHelper.cs
│       ├── Screenshots/
│       │   └── ScreenshotManager.cs
│       └── Logging/
│           ├── ILogger.cs
│           └── TestLogger.cs
│
├── tests/
│   ├── TestAutomationFramework.UI.Tests/
│   │   ├── Features/
│   │   │   ├── Login/
│   │   │   │   ├── LoginTests.cs
│   │   │   │   └── LoginTestData.cs
│   │   │   ├── UserManagement/
│   │   │   │   └── UserManagementTests.cs
│   │   │   └── Products/
│   │   │       └── ProductTests.cs
│   │   ├── TestData/
│   │   │   ├── users.csv
│   │   │   ├── products.json
│   │   │   └── test-config.json
│   │   └── Fixtures/
│   │       ├── UITestFixture.cs
│   │       └── TestDataFixture.cs
│   │
│   ├── TestAutomationFramework.API.Tests/
│   │   ├── Features/
│   │   │   ├── Users/
│   │   │   │   ├── UserApiTests.cs
│   │   │   │   └── UserTestData.cs
│   │   │   └── Products/
│   │   │       └── ProductApiTests.cs
│   │   ├── TestData/
│   │   │   ├── api-test-data.json
│   │   │   └── mock-responses.json
│   │   └── Fixtures/
│   │       └── ApiTestFixture.cs
│   │
│   └── TestAutomationFramework.Integration.Tests/
│       ├── EndToEnd/
│       │   └── E2ETests.cs
│       └── Database/
│           └── DatabaseTests.cs
│
├── config/
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── appsettings.Staging.json
│   ├── appsettings.Production.json
│   └── allure.config.json
│
├── scripts/
│   ├── run-tests.ps1
│   ├── generate-allure-report.ps1
│   └── setup-environment.ps1
│
├── docker/
│   ├── Dockerfile
│   └── docker-compose.yml
│
├── .github/
│   └── workflows/
│       ├── ui-tests.yml
│       ├── api-tests.yml
│       └── integration-tests.yml
│
├── TestAutomationFramework.sln
├── .gitignore
├── README.md
├── Directory.Build.props
└── global.json
```

## Core Architecture Components

### 1. **Core Layer (TestAutomationFramework.Core)**
- **Configuration Management**: Centralized configuration handling with environment-specific settings
- **Models & DTOs**: Shared data models across all layers
- **Utilities**: Common helper functions and extensions
- **Constants**: Application-wide constants and enums

### 2. **UI Layer (TestAutomationFramework.UI)**
- **Page Object Model**: Implements POM pattern with Playwright
- **Component-Based Architecture**: Reusable UI components
- **Driver Management**: Browser initialization and lifecycle management
- **Locator Strategy**: Centralized element locators

### 3. **API Layer (TestAutomationFramework.API)**
- **Client Pattern**: Dedicated API clients for different services
- **Request/Response Models**: Strongly typed API contracts
- **Mock Server Integration**: WireMock.NET for API mocking
- **Authentication Handling**: Centralized auth management

### 4. **Database Layer (TestAutomationFramework.Database)**
- **Repository Pattern**: Data access abstraction
- **Entity Framework/Dapper**: Flexible ORM approach
- **Migration Support**: Database versioning
- **Test Data Management**: Database seeding and cleanup

### 5. **Reporting Layer (TestAutomationFramework.Reporting)**
- **Allure Integration**: Rich test reporting
- **Screenshot Management**: Automated failure screenshots
- **Logging**: Structured logging throughout framework

## Key Design Patterns

### 1. **Page Object Model (POM)**
```csharp
public class LoginPage : BasePage
{
    private readonly LoginPageLocators _locators;
    
    public LoginPage(IPage page) : base(page)
    {
        _locators = new LoginPageLocators();
    }
    
    public async Task<bool> LoginAsync(string username, string password)
    {
        await Page.FillAsync(_locators.UsernameInput, username);
        await Page.FillAsync(_locators.PasswordInput, password);
        await Page.ClickAsync(_locators.LoginButton);
        
        return await Page.IsVisibleAsync(_locators.DashboardIndicator);
    }
}
```

### 2. **Factory Pattern for Browser Management**
```csharp
public class BrowserFactory
{
    public static async Task<IBrowser> CreateBrowserAsync(BrowserType browserType)
    {
        var playwright = await Playwright.CreateAsync();
        
        return browserType switch
        {
            BrowserType.Chromium => await playwright.Chromium.LaunchAsync(),
            BrowserType.Firefox => await playwright.Firefox.LaunchAsync(),
            BrowserType.WebKit => await playwright.Webkit.LaunchAsync(),
            _ => throw new NotSupportedException($"Browser {browserType} is not supported")
        };
    }
}
```

### 3. **Repository Pattern for Data Access**
```csharp
public interface IUserRepository : IBaseRepository<UserEntity>
{
    Task<UserEntity> GetByEmailAsync(string email);
    Task<List<UserEntity>> GetActiveUsersAsync();
}

public class UserRepository : BaseRepository<UserEntity>, IUserRepository
{
    public UserRepository(TestDbContext context) : base(context) { }
    
    public async Task<UserEntity> GetByEmailAsync(string email)
    {
        return await Context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}
```

### 4. **Builder Pattern for Test Data**
```csharp
public class UserBuilder
{
    private User _user = new User();
    
    public UserBuilder WithEmail(string email)
    {
        _user.Email = email;
        return this;
    }
    
    public UserBuilder WithRandomData()
    {
        _user.Email = RandomDataGenerator.GenerateEmail();
        _user.FirstName = RandomDataGenerator.GenerateFirstName();
        return this;
    }
    
    public User Build() => _user;
}
```

## Configuration Management

### appsettings.json Structure
```json
{
  "TestConfiguration": {
    "Browser": "Chromium",
    "Headless": false,
    "BaseUrl": "https://dev.example.com",
    "Timeout": 30000,
    "Screenshots": {
      "OnFailure": true,
      "Directory": "screenshots"
    },
    "Database": {
      "ConnectionString": "Server=localhost;Database=TestDB;Trusted_Connection=true;",
      "Provider": "SqlServer"
    },
    "Api": {
      "BaseUrl": "https://api.dev.example.com",
      "Timeout": 30,
      "MockServer": {
        "Enabled": false,
        "Port": 9999
      }
    },
    "Allure": {
      "Directory": "allure-results",
      "Environment": "Development"
    }
  }
}
```

## Best Practices & Recommendations

### 1. **Test Organization**
- Group tests by feature/functionality
- Use descriptive test names following AAA pattern
- Implement proper test data management
- Separate positive and negative test scenarios

### 2. **Playwright Best Practices**
- Use auto-waiting capabilities
- Implement proper element selection strategies
- Handle dynamic content appropriately
- Use page.locator() for better element handling

### 3. **API Testing Strategies**
- Implement contract testing
- Use proper HTTP status code validation
- Test both happy path and error scenarios
- Implement request/response logging

### 4. **Database Testing**
- Implement proper test data cleanup
- Use transactions for test isolation
- Create database snapshots for complex scenarios
- Test data integrity and constraints

### 5. **Reporting & Monitoring**
- Integrate Allure reporting with CI/CD
- Implement proper logging levels
- Capture screenshots on failures
- Track test execution metrics

### 6. **CI/CD Integration**
- Separate test suites (smoke, regression, integration)
- Implement parallel test execution
- Use Docker for consistent environments
- Implement proper artifact storage

### 7. **Maintenance & Scalability**
- Regular framework updates
- Code review processes
- Documentation maintenance
- Performance monitoring

## Framework Benefits

### **Maintainability**
- Clean separation of concerns
- Reusable components
- Centralized configuration
- Consistent coding patterns

### **Scalability**
- Modular architecture
- Easy to add new test types
- Supports multiple environments
- Parallel execution ready

### **Reliability**
- Robust error handling
- Comprehensive logging
- Automatic retries
- Environment isolation

### **Efficiency**
- Fast test execution
- Minimal setup overhead
- Rich reporting capabilities
- CI/CD integration ready

This architecture provides a solid foundation for building a comprehensive, maintainable, and scalable test automation framework using your specified tech stack.