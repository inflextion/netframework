# AutomationFramework (No DI)

A high-level structure for a .NET 9 test automation framework using xUnit and Playwright—without a dependency-injection container.

---

## Solution & Project Layout

```text
AutomationFramework.sln
│
├── src
│   ├── Core/                             # Shared abstractions & utilities
│   │   ├── Logging/                      # Static logger helper (e.g. Serilog static Log)
│   │   ├── Configuration/                # Static ConfigManager reads JSON/env
│   │   ├── Models/                       # DTOs for API, UI, DB
│   │   └── Utilities/                    # Retry helpers, random data, waits
│   │
│   ├── API/                              # API test harness
│   │   ├── Clients/                      # HttpClient wrappers you `new` up
│   │   ├── Builders/                     # Fluent builders for requests
│   │   ├── Deserializers/                # Response parsers
│   │   └── Fixtures/                     # xUnit fixtures with manual setup
│   │
│   ├── UI/                               # UI test harness
│   │   ├── PlaywrightSetup/              # Static PlaywrightLauncher
│   │   ├── Pages/                        # Page Object Models
│   │   └── Helpers/                      # Selector & wait helpers
│   │
│   ├── Data/                             # DB integration & test data
│   │   ├── DbContextFactory/             # Manual factory for DbContext/Dapper
│   │   ├── Repositories/                 # CRUD helpers (instantiated directly)
│   │   └── SeedData/                     # Scripts to seed/reset
│   │
│   └── Tests/                            # xUnit test projects
│       ├── Tests.API/                    # API test classes
│       ├── Tests.UI/                     # UI test classes
│       ├── Tests.Integration/            # End-to-end crossing layers
│       └── Tests.Mocks/                  # Moq setups and stub providers

```

## How It Works Without DI

1. **Configuration**  
   - **`ConfigManager`**: Static class that reads `appsettings.json` (plus env overrides) on first access and exposes strongly-typed settings.  
   - Usage: `ConfigManager.Get("ApiBaseUrl")` or `ConfigManager.Section<T>("Db")`.

2. **Logging**  
   - **`Log`**: Static wrapper around Serilog or `LoggerFactory`.  
   - Usage: `Log.Information("...")`, no injection required.

3. **API Clients**  
   - Constructed manually in xUnit fixtures:
     ```csharp
     var http    = new HttpClient { BaseAddress = new Uri(ConfigManager.ApiBaseUrl) };
     var client  = new UserApiClient(http, new RetryHelper());
     ```
   - Helpers like `RetryHelper` are `new`’d or static.

4. **UI Setup**  
   - **`PlaywrightLauncher`**: Static helper initializing a singleton `IBrowser`.  
   - In tests:
     ```csharp
     var browser = PlaywrightLauncher.Launch(headless: true);
     var page    = browser.NewPage();
     var login   = new LoginPage(page);
     ```
   - Tear-down: Close browser in fixture’s `Dispose()`.

5. **Database Access**  
   - **`DbContextFactory`**: Returns a new `TestDbContext` or Dapper `IDbConnection` using connection strings from `ConfigManager`.  
   - Repositories instantiated directly:
     ```csharp
     var ctx  = DbContextFactory.Create();
     var repo = new UserRepository(ctx);
     ```

6. **Mocking & Stubs**  
   - Use Moq or NSubstitute in unit tests, passing mocks directly into constructors:
     ```csharp
     var mailMock = new Mock<IEmailService>();
     var svc      = new NotificationService(mailMock.Object, Log);
     ```
   - No container—just `new` the service under test.

7. **Test Organization & Fixtures**  
   - xUnit fixtures (`[Collection]`, `IClassFixture<T>`) handle manual setup/teardown.  
   - Shared resources (e.g. browser, seeded DB) are created via static factories in fixture constructors.

---

## Pros & Cons

| Pros                                      | Cons                                         |
| ----------------------------------------- | -------------------------------------------- |
| Simple to understand—no container magic.  | Wiring can become verbose at scale.          |
| Total control over object lifetimes.      | Swapping implementations globally is manual. |
| No extra DI-package dependencies.         | Constructor chains require manual refactoring.|

---

## Key Patterns to Adopt

- **Factory/Builder Pattern**  
  Encapsulate complex object creation behind static `XFactory.Create()` methods or fluent builders.

- **Singletons for Shared Resources**  
  Use static lazy initializers for browsers, configuration, etc.

- **Explicit Constructor Injection**  
  Design classes to accept dependencies via constructors, then wire them manually in fixtures.

---