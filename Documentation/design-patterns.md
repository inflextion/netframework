# Design Patterns in the Test Automation Framework

This document outlines the key design patterns implemented in our .NET test automation framework, providing concrete examples from the codebase.

## 1. Factory Pattern (True Factory)

**Purpose**: Creates objects without specifying the exact class to create - returns interface, not concrete type.

### Problem It Solves
Many codebases implement "pseudo-factories" that centralize object creation but still expose concrete types:

```csharp
// ❌ Pseudo-Factory - not a true factory pattern
public static ProductApiClient CreateProductClient()  // Returns concrete type
{
    return new ProductApiClient(restClient);  // Client knows exact implementation
}
```

**Issues with this approach:**
- **Tight Coupling**: Tests depend on specific concrete classes
- **Hard to Extend**: Adding new API types requires changing existing code
- **No Runtime Flexibility**: Cannot switch implementations dynamically
- **Violates Open/Closed Principle**: Must modify factory for each new type

### Our True Factory Implementation
```csharp
// API/Clients/ApiClientFactory.cs
public static class ApiClientFactory
{
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
```

### Benefits & Problem Resolution

#### **1. True Abstraction**
```csharp
// ✅ Tests work with interface, not concrete types
IApiClient client = ApiClientFactory.CreateProductClient();
var result = await client.PostAsync<Product>(product, "/api/products");
```

#### **2. Runtime Flexibility**
```csharp
// ✅ Can switch client types based on test needs
var clientType = needsAuthentication ? ApiClientType.User : ApiClientType.Product;
var client = ApiClientFactory.CreateClient(clientType);
```

#### **3. Easy Extension** 
Adding new API domains (Analytics, Checkout, Logs) requires:
- ✅ Add enum value: `ApiClientType.Analytics`
- ✅ Add case to switch statement
- ❌ **Zero changes to existing test code**

#### **4. End-to-End Scenarios**
```csharp
[Fact]
public async Task Should_Complete_Full_User_Journey()
{
    // Multiple API clients working together seamlessly
    var userClient = ApiClientFactory.CreateUserClient();
    var productClient = ApiClientFactory.CreateProductClient();
    
    var loginResponse = await userClient.PostAsync<LoginResponse>(credentials, "/api/users/login");
    var products = await productClient.GetAsync<List<Product>>("/api/products");
    
    // Complete workflow with proper abstraction
    Assert.Equal("Login successful", loginResponse.Message);
    Assert.NotEmpty(products);
}
```

#### **5. Centralized Configuration**
All clients share the same base configuration (timeout, base URL, error handling) but can have specialized behavior through their implementations.

### Real-World Impact
This pattern enabled us to support **5 different API domains** in our backend:
- **Products API** (`/api/products`) - CRUD operations
- **Users API** (`/api/users`) - Authentication and user management
- **Analytics API** (`/api/analytics`) - Business reporting (future)
- **Checkout API** (`/api/checkout`) - Order processing (future)
- **Logs API** (`/api/logs`) - System monitoring (future)

Each domain gets its own specialized client while maintaining a consistent interface for tests.

## 2. Builder Pattern

**Purpose**: Constructs complex objects step by step with a fluent interface.

### Problem It Solves
Creating test objects with multiple optional parameters becomes unwieldy:

```csharp
// ❌ Constructor with many parameters - hard to read and maintain
var product = new ProductRequest(
    id: 123,
    name: "Laptop",
    category: "Electronics", 
    price: 999.99m,
    description: "Gaming laptop",
    inStock: true,
    tags: new[] { "gaming", "laptop" },
    metadata: new Dictionary<string, object> { {"brand", "Dell"} }
);

// ❌ Multiple constructors lead to constructor explosion
public ProductRequest(string name, string category) { }
public ProductRequest(string name, string category, decimal price) { }
public ProductRequest(string name, string category, decimal price, bool inStock) { }
// ... dozens of constructor overloads
```

### Our Builder Implementation
```csharp
// API/Builders/ProductRequestBuilder.cs
public class ProductRequestBuilder
{
    private ProductRequest _product = new();
    
    public ProductRequestBuilder WithFakeData() 
    { 
        _product.Id = TestDataFaker.FakeId();
        _product.Name = TestDataFaker.FakeProductName();
        _product.Category = TestDataFaker.FakeCategory();
        _product.Price = TestDataFaker.FakePrice();
        return this;
    }
    
    public ProductRequestBuilder WithCategory(string category) 
    { 
        _product.Category = category;
        return this;
    }
    
    public ProductRequestBuilder WithFakePrice(decimal min, decimal max) 
    { 
        _product.Price = TestDataFaker.FakePrice(min, max);
        return this;
    }
    
    public ProductRequest Build() => _product;
}

// Usage in tests - clean and readable
var product = new ProductRequestBuilder()
    .WithFakeId()           // Random ID
    .WithFakeName()         // Random product name
    .WithCategory("Laptops") // Specific category
    .WithFakePrice(100, 500) // Random price in range
    .Build();
```

### Benefits & Problem Resolution

#### **1. Flexible Object Construction**
```csharp
// ✅ Mix real and fake data as needed
var basicProduct = new ProductRequestBuilder()
    .WithFakeData()
    .Build();

var specificProduct = new ProductRequestBuilder()
    .WithFakeData()
    .WithCategory("Gaming")        // Override fake category
    .WithPrice(1999.99m)          // Override fake price
    .Build();
```

#### **2. Readable Test Code**
Method names clearly express intent, making tests self-documenting:
```csharp
// ✅ Clear what each test is trying to achieve
var cheapProduct = new ProductRequestBuilder().WithFakeData().WithPriceBetween(10, 50).Build();
var expensiveProduct = new ProductRequestBuilder().WithFakeData().WithPriceBetween(1000, 5000).Build();
var outOfStockProduct = new ProductRequestBuilder().WithFakeData().WithInStock(false).Build();
```

#### **3. Fresh Test Data Every Run**
```csharp
// ✅ No hardcoded values - fresh data prevents test coupling
for (int i = 0; i < 100; i++)
{
    var uniqueProduct = new ProductRequestBuilder().WithFakeData().Build();
    // Each iteration gets completely unique data
}
```

#### **4. Easy Maintenance**
Adding new fields to ProductRequest only requires updating the builder, not dozens of test constructors.

### Real-World Usage in Our Tests
```csharp
[Theory]
[InlineData("Electronics")]
[InlineData("Clothing")]
[InlineData("Books")]
public async Task Should_Create_Products_In_Different_Categories(string category)
{
    // Builder makes it easy to test category-specific logic
    var product = new ProductRequestBuilder()
        .WithFakeData()
        .WithCategory(category)  // Only thing that changes per test
        .Build();
        
    var result = await Client.PostAsync<ProductRequest>(product, "/api/products");
    Assert.Equal(category, result.Category);
}
```

## 3. Template Method Pattern

**Purpose**: Defines skeleton of algorithm in base class, subclasses override specific steps.

### BaseApiTest<TClient>
```csharp
// Tests/Base/BaseApiTest.cs
public abstract class BaseApiTest<TClient> : IAsyncLifetime
{
    public virtual async Task InitializeAsync()
    {
        Client = CreateClient();    // Template method - implemented by subclass
        await OnSetupAsync();       // Hook for subclass customization
    }
    
    protected abstract TClient CreateClient();  // Must be implemented by subclass
    protected virtual Task OnSetupAsync() { }   // Optional override
}

// Tests/Base/ProductApiTestBase.cs
public abstract class ProductApiTestBase : BaseApiTest<ProductApiClient>
{
    protected override ProductApiClient CreateClient()  // Implements template method
    {
        return ApiClientFactory.CreateProductClient();
    }
}
```

**Benefits**: Consistent test lifecycle, reusable setup/teardown logic, extensible for different API clients.

## 4. Page Object Model (POM)

**Purpose**: Encapsulates web page elements and actions in separate classes.

### BasePage and Specific Pages
```csharp
// UI/Pages/BasePage.cs
public abstract class BasePage
{
    protected IPage Page { get; }
    protected abstract string PageUrl { get; }
    
    public virtual async Task NavigateToAsync()
    {
        await Page.GotoAsync(PageUrl);
    }
}

// UI/Pages/WebElementsPage.cs
public class WebElementsPage : BasePage
{
    // ILocator properties initialized in constructor
    public ILocator SubmitButton { get; }
    
    public WebElementsPage(IPage page, PlaywrightSettings settings, ILogger logger) 
        : base(page, settings, logger)
    {
        SubmitButton = Page.Locator("#submit-button");
    }
    
    public async Task ClickButtonAsync()
    {
        await SubmitButton.ClickAsync();
    }
}
```

**Benefits**: Separation of test logic from UI implementation, reusable page actions, maintainable locators.

## 5. Repository Pattern

**Purpose**: Encapsulates data access logic behind an interface.

### Product Repository
```csharp
// Data/Repositories/IProductRepository.cs
public interface IProductRepository
{
    Task<Product> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task<List<Product>> GetByCategoryAsync(string category);
}

// Data/Repositories/ProductRepository.cs
public class ProductRepository : IProductRepository
{
    private readonly TestDbContext _context;
    
    public async Task<Product> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }
}
```

**Benefits**: Testable data access, abstraction over database operations, swappable implementations.

## 6. Singleton Pattern

**Purpose**: Ensures only one instance exists and provides global access.

### ConfigManager
```csharp
// Core/Config/ConfigManager.cs
public static class ConfigManager
{
    private static readonly IConfigurationRoot _configuration;

    static ConfigManager() => _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

    public static T Get<T>(string key) => _configuration.GetValue<T>(key);
    public static T GetSection<T>(string key) where T : new() => _configuration.GetSection(key).Get<T>();
}

// Usage throughout the framework
var timeout = ConfigManager.Get<int>("ApiClient:TimeoutMs");
```

**Benefits**: Simple static access, thread-safe, environment variable support, consistent settings across tests.

## 7. Strategy Pattern (Implicit)

**Purpose**: Defines family of algorithms and makes them interchangeable.

### Browser Selection
```csharp
// Core/Enums/BrowserList.cs
public enum BrowserList
{
    Chromium,
    Firefox,
    Webkit
}

// UI/PlaywrightSetup/PlaywrightSetup.cs
public static async Task<IBrowser> CreateBrowserAsync(BrowserList browserType)
{
    return browserType switch
    {
        BrowserList.Chromium => await Playwright.Chromium.LaunchAsync(options),
        BrowserList.Firefox => await Playwright.Firefox.LaunchAsync(options),
        BrowserList.Webkit => await Playwright.Webkit.LaunchAsync(options),
        _ => throw new ArgumentException($"Unsupported browser: {browserType}")
    };
}
```

**Benefits**: Runtime browser selection, easy to add new browsers, configurable test execution.

## 8. Dependency Injection (Manual)

**Purpose**: Provides dependencies from external source rather than creating them internally.

### Client Injection in Tests
```csharp
// BaseClient receives RestClient as dependency
public abstract class BaseClient : IDisposable
{
    protected BaseClient(RestClient restClient)  // Dependency injected
    {
        RestClient = restClient ?? throw new ArgumentNullException(nameof(restClient));
    }
}

// ProductApiClient depends on RestClient
public class ProductApiClient : BaseClient
{
    public ProductApiClient(RestClient restClient) : base(restClient) { }
}
```

**Benefits**: Testable components, loose coupling, configurable dependencies.

## Pattern Benefits Summary

| Pattern | Primary Benefit | Example Usage |
|---------|----------------|---------------|
| **Factory** | Centralized object creation | `ApiClientFactory.CreateProductClient()` |
| **Builder** | Flexible object construction | `ProductRequestBuilder.WithFakeData().Build()` |
| **Template Method** | Consistent algorithm with variation points | `BaseApiTest.InitializeAsync()` |
| **Page Object** | UI abstraction and reusability | `WebElementsPage.ClickButtonAsync()` |
| **Repository** | Data access abstraction | `ProductRepository.GetByIdAsync()` |
| **Singleton** | Global configuration access | `ConfigManager.Get<int>()` |
| **Strategy** | Runtime algorithm selection | Browser type selection |
| **Dependency Injection** | Loose coupling and testability | RestClient injection |

These patterns work together to create a maintainable, scalable, and testable automation framework that follows SOLID principles and industry best practices.