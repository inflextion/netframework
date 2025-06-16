# Workshop Tasks for Beginners

Welcome to the .NET Test Automation Framework workshop! These tasks are designed for beginners to gradually learn programming concepts while working with a real test automation framework.

## Prerequisites
- Basic understanding of C# syntax (variables, methods, classes)
- Framework already set up and running
- VS Code or Visual Studio installed

---

## 🌟 Level 1: Getting Started (Beginner)

### Task 1.1: Fix the Broken Test
**Objective:** Learn to inspect web pages and update selectors
**Time:** 15 minutes

The `AdvancedPageTests.cs` test is currently failing because the selectors don't match the actual webpage.

**Steps:**
1. Run the failing test and observe the error
2. Navigate to `http://localhost:3000/advanced` in your browser
3. Right-click → Inspect Element to find the correct selectors
4. Update the selectors in `AdvancedWebElementsPage.cs`
5. Run the test again to verify it passes

**What you'll learn:** 
- How CSS selectors work
- Reading error messages
- Browser developer tools

---

### Task 1.2: Add a New Assertion Method
**Objective:** Extend the BasePage class with a simple method
**Time:** 10 minutes

Add a new method to check if an element is visible.

**Instructions:**
1. Open `UI/Pages/BasePage.cs`
2. Add this method after the existing methods:
```csharp
protected async Task<bool> IsVisibleAsync(string selector)
{
    try
    {
        return await Page.Locator(selector).IsVisibleAsync();
    }
    catch (Exception ex)
    {
        Logger.Error(ex, "Failed to check visibility of selector '{Selector}'", selector);
        return false;
    }
}
```
3. Use this method in any page object to check if elements are visible

**What you'll learn:**
- Method structure and return types
- Error handling with try-catch
- Boolean values

---

### Task 1.3: Create Interactive Form Tests
**Objective:** Test multiple form interactions using WebElementsPage
**Time:** 25 minutes

Create comprehensive tests for the WebElements page that test different types of form controls.

**Instructions:**
1. Create a new file: `Tests/Tests.UI/InteractiveFormTests.cs`
2. Implement these test scenarios:

```csharp
using atf.Core.Models;
using atf.Core.Enums;
using atf.UI.Pages;
using Microsoft.Playwright;
using Xunit.Abstractions;
using Xunit;
using atf.Core.Config;

namespace atf.Tests.Tests.UI
{
    public class InteractiveFormTests : BaseUiTest
    {
        private readonly PlaywrightSettings _settings;
        
        public InteractiveFormTests(ITestOutputHelper output) : base(output) 
        {
            _settings = ConfigManager.GetSection<PlaywrightSettings>("Playwright");
        }

        [Fact]
        public async Task Should_Handle_Counter_Operations_Correctly()
        {
            // TODO: Navigate to /webelements page
            var webElementsPage = new WebElementsPage(Page, _settings, TestLogger.Logger);
            
            // TODO: Click increment button 3 times
            // TODO: Verify counter shows "3"
            // TODO: Click decrement button once  
            // TODO: Verify counter shows "2"
            // TODO: Click reset button
            // TODO: Verify counter shows "0"
            // TODO: Take screenshot after each major step
            
            // Example structure:
            // await webElementsPage.GoToAsync("/webelements");
            // for (int i = 0; i < 3; i++)
            // {
            //     await webElementsPage.ClickCounterIncrementAsync();
            // }
            // var counterValue = await webElementsPage.GetCounterValueAsync();
            // Assert.Equal("3", counterValue);
        }

        [Theory]
        [InlineData("Option1", "Option1")]
        [InlineData("Option2", "Option2")] 
        [InlineData("Option3", "Option3")]
        public async Task Should_Select_Dropdown_Options_And_Display_Output(string optionValue, string expectedOutput)
        {
            // TODO: Navigate to /webelements page
            // TODO: Select the dropdown option using optionValue
            // TODO: Get the dropdown output text
            // TODO: Assert the output contains expectedOutput
            // TODO: Take screenshot with option name in filename
            
            var webElementsPage = new WebElementsPage(Page, _settings, TestLogger.Logger);
            
            // Your implementation here...
        }
    }
}
```

**What you'll learn:**
- Theory tests with multiple data sets
- Loop structures in tests
- Counter and dropdown interactions
- Sequential test steps

---

### Task 1.4: Create Multi-Element Interaction Tests  
**Objective:** Test complex interactions using AdvancedWebElementsPage
**Time:** 30 minutes

Build tests that interact with multiple elements and verify their relationships.

**Instructions:**
1. Create a new file: `Tests/Tests.UI/MultiElementInteractionTests.cs`
2. Implement these advanced scenarios:

```csharp
using atf.Core.Models;
using atf.Core.Enums;
using atf.UI.Pages;
using Microsoft.Playwright;
using Xunit.Abstractions;
using Xunit;
using atf.Core.Config;

namespace atf.Tests.Tests.UI
{
    public class MultiElementInteractionTests : BaseUiTest
    {
        private readonly PlaywrightSettings _settings;
        
        public MultiElementInteractionTests(ITestOutputHelper output) : base(output) 
        {
            _settings = ConfigManager.GetSection<PlaywrightSettings>("Playwright");
        }

        [Fact]
        public async Task Should_Handle_Checkbox_Group_Selections()
        {
            var advancedPage = new AdvancedWebElementsPage(Page, _settings, TestLogger.Logger);
            
            // TODO: Navigate to /advanced page (fix selectors first!)
            // TODO: Check the first checkbox (index 0)
            // TODO: Check the third checkbox (index 2)  
            // TODO: Verify output shows both selections
            // TODO: Uncheck the first checkbox
            // TODO: Verify output now only shows the third checkbox
            // TODO: Take screenshots before and after changes
            
            // Example structure:
            // await advancedPage.GoToAsync("/advanced");
            // await advancedPage.CheckCheckboxAsync(0);
            // await advancedPage.CheckCheckboxAsync(2);
            // var output = await advancedPage.GetCheckboxOutputAsync();
            // Assert.Contains("expected text", output);
        }

        [Fact]
        public async Task Should_Coordinate_Text_Input_With_Other_Elements()
        {
            var advancedPage = new AdvancedWebElementsPage(Page, _settings, TestLogger.Logger);
            
            // TODO: Navigate to /advanced page
            // TODO: Enter text "Workshop Test" in text input
            // TODO: Select a radio button (index 1)
            // TODO: Check a checkbox (index 0)
            // TODO: Verify all three outputs show their respective values
            // TODO: Clear the text input and enter new text "Updated Text"
            // TODO: Verify text output updated but radio/checkbox outputs unchanged
            // TODO: Take final screenshot showing all interactions
            
            // This test verifies that different form elements work independently
            // and maintain their state when other elements are modified
        }

        [Theory]
        [InlineData(BrowserList.Firefox, "Firefox Test Data")]
        [InlineData(BrowserList.Edge, "Edge Test Data")]
        [InlineData(BrowserList.Webkit, "Webkit Test Data")]
        public async Task Should_Work_Consistently_Across_Browsers(BrowserList browserType, string testData)
        {
            // TODO: Launch specific browser using LaunchBrowserAsync(browserType)
            // TODO: Create page object with browser context in logger
            // TODO: Navigate to /advanced page  
            // TODO: Enter the testData in text input
            // TODO: Select radio button index 0
            // TODO: Verify text output contains testData
            // TODO: Verify radio output shows expected selection
            // TODO: Take screenshot with browser name in filename
            
            var caseLogger = TestLogger.Logger.ForContext("Browser", browserType);
            var advancedPage = new AdvancedWebElementsPage(Page, _settings, caseLogger);
            
            // Your cross-browser implementation here...
        }
    }
}
```

**What you'll learn:**
- Multi-step interactions
- State management between elements  
- Cross-browser testing patterns
- Complex assertion scenarios

---

### Task 1.5: Create Your First Homepage Test
**Objective:** Write a simple navigation test
**Time:** 15 minutes

Create a basic test that verifies navigation between pages.

**Instructions:**
1. Create a new file: `Tests/Tests.UI/NavigationTests.cs`
2. Copy this template and fill in the missing parts:
```csharp
using atf.Core.Models;
using Microsoft.Playwright;
using Xunit.Abstractions;
using Xunit;
using atf.Core.Config;

namespace atf.Tests.Tests.UI
{
    public class NavigationTests : BaseUiTest
    {
        private readonly PlaywrightSettings _settings;
        
        public NavigationTests(ITestOutputHelper output) : base(output) 
        {
            _settings = ConfigManager.GetSection<PlaywrightSettings>("Playwright");
        }

        [Fact]
        public async Task Should_Navigate_Between_Pages_Successfully()
        {
            // TODO: Navigate to the homepage "/"
            // TODO: Take a screenshot called "homepage-loaded"
            // TODO: Navigate to "/webelements"  
            // TODO: Verify URL contains "webelements"
            // TODO: Navigate to "/advanced"
            // TODO: Verify URL contains "advanced"
            // TODO: Take final screenshot
            
            // Example assertions:
            // Assert.Contains("webelements", Page.Url);
            // Assert.Contains("advanced", Page.Url);
        }
    }
}
```

**What you'll learn:**
- Basic navigation
- URL verification
- Simple test structure

---

## 🚀 Level 2: Building Confidence (Beginner-Intermediate)

### Task 2.1: Add Validation to ProductRequestBuilder
**Objective:** Learn about data validation and defensive programming
**Time:** 15 minutes

Add validation to prevent invalid product data.

**Instructions:**
1. Open `API/Builders/ProductRequestBuilder.cs`
2. Find the `WithPrice` method
3. Add validation to ensure price is positive:
```csharp
public ProductRequestBuilder WithPrice(decimal price)
{
    if (price <= 0)
    {
        throw new ArgumentException("Price must be greater than zero", nameof(price));
    }
    _product.Price = price;
    return this;
}
```
4. Write a test to verify this validation works

**What you'll learn:**
- Input validation
- Exception handling
- Defensive programming

---

### Task 2.2: Create a Simple Data Helper
**Objective:** Build a utility method for generating test data
**Time:** 20 minutes

Create a helper method that generates realistic email addresses.

**Instructions:**
1. Open `Core/Utils/TestDataFaker.cs`
2. Add this new method:
```csharp
public static string FakeCompanyEmail(string companyName = null)
{
    companyName = companyName ?? FakeCompany();
    // TODO: Create an email like "user@company.com"
    // Hint: Use Faker.Internet.Email() and string manipulation
    // Remove spaces and make lowercase for the domain
    return ""; // Replace with your implementation
}

public static string FakeCompany()
{
    return Faker.Company.CompanyName();
}
```
3. Use this method in a test to create realistic test data

**What you'll learn:**
- String manipulation
- Method parameters and defaults
- Utility classes

---

### Task 2.3: Add Screenshot Comparison
**Objective:** Learn about visual testing concepts
**Time:** 25 minutes

Add a method to take and compare screenshots.

**Instructions:**
1. Open `Tests/Tests.UI/BaseUiTest.cs`
2. Add this method:
```csharp
protected async Task<bool> CompareScreenshotAsync(string testName, string elementSelector = null)
{
    var baselineFolder = Path.Combine("Screenshots", "Baseline");
    var currentFolder = Path.Combine("Screenshots", "Current");
    
    // TODO: Create directories if they don't exist
    // TODO: Take a screenshot and save to current folder
    // TODO: Check if baseline exists, if not, copy current as baseline
    // TODO: Return true if screenshots match (or baseline is missing)
    
    // For now, just return true
    return true;
}
```

**What you'll learn:**
- File system operations
- Directory handling
- Visual testing concepts

---

## 🔥 Level 3: Real-World Skills (Intermediate)

### Task 3.1: Create a Configuration Validator
**Objective:** Learn about configuration management and validation
**Time:** 30 minutes

Create a utility that validates your test configuration on startup.

**Instructions:**
1. Create a new file: `Core/Utils/ConfigValidator.cs`
2. Implement this class:
```csharp
public static class ConfigValidator
{
    public static void ValidateTestConfiguration()
    {
        // TODO: Check if BaseUrl is set and reachable
        // TODO: Verify browser type is valid
        // TODO: Check if required timeouts are positive numbers
        // TODO: Throw descriptive exceptions for any issues
        
        // Example structure:
        var playwrightSettings = ConfigManager.GetSection<PlaywrightSettings>("Playwright");
        
        if (string.IsNullOrEmpty(playwrightSettings.BaseUrl))
        {
            throw new InvalidOperationException("BaseUrl must be configured in appsettings.json");
        }
        
        // Add more validations...
    }
}
```
3. Call this method in `BaseUiTest.InitializeAsync()`

**What you'll learn:**
- Configuration validation
- Error handling with meaningful messages
- Startup validation patterns

---

### Task 3.2: Implement Retry Logic
**Objective:** Learn about resilience patterns in testing
**Time:** 35 minutes

Add retry capability to the API client for handling flaky network calls.

**Instructions:**
1. Open `API/Clients/BaseClient.cs`
2. Add this retry method:
```csharp
protected async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> operation, int maxRetries = 3)
{
    var attempt = 1;
    Exception lastException = null;
    
    while (attempt <= maxRetries)
    {
        try
        {
            // TODO: Log the attempt number
            // TODO: Execute the operation
            // TODO: Return result if successful
        }
        catch (Exception ex)
        {
            // TODO: Store the exception
            // TODO: Log the failure
            // TODO: Wait before retrying (exponential backoff?)
            // TODO: Increment attempt counter
        }
    }
    
    // TODO: Throw the last exception if all retries failed
    throw lastException ?? new Exception("Operation failed after retries");
}
```
3. Use this method in your API calls

**What you'll learn:**
- Retry patterns
- Exception handling
- Resilient programming

---

### Task 3.3: Create a Test Data Factory
**Objective:** Learn about the Factory pattern and organized test data
**Time:** 40 minutes

Build a factory that creates different types of test users and products.

**Instructions:**
1. Create a new file: `Tests/Helpers/TestDataFactory.cs`
2. Implement different scenarios:
```csharp
public static class TestDataFactory
{
    public enum UserType
    {
        Standard,
        Premium,
        Admin,
        Inactive
    }
    
    public enum ProductType
    {
        Electronics,
        Clothing,
        Books,
        OutOfStock,
        Expensive
    }
    
    public static User CreateUser(UserType userType)
    {
        // TODO: Create different user configurations based on type
        // Standard: Regular user with basic permissions
        // Premium: User with premium features
        // Admin: User with admin rights
        // Inactive: Disabled user account
        return new User();
    }
    
    public static Product CreateProduct(ProductType productType)
    {
        // TODO: Create different product configurations
        // Electronics: High-tech gadgets
        // Clothing: Fashion items
        // Books: Literature and educational
        // OutOfStock: Products with zero inventory
        // Expensive: Luxury items with high prices
        return new Product();
    }
}
```
3. Use these factories in your tests instead of hardcoded data

**What you'll learn:**
- Factory design pattern
- Enum usage
- Organized test data management

---

### Task 3.4: Implement Generic AllureHelper Method
**Objective:** Refactor AllureHelper to support any API model type
**Time:** 30 minutes

Make the AllureHelper.AttachString method generic to work with any API request model, not just ProductRequest.

**Current Problem:**
The `AllureHelper.AttachString(string name, ProductRequest request)` method in `/Tests/Helpers/AllureHelper.cs:33` is hardcoded to only accept ProductRequest objects, limiting its reusability with other API models like LoginRequest.

**Instructions:**
1. Open `Tests/Helpers/AllureHelper.cs`
2. Replace the ProductRequest-specific method with a generic version:
```csharp
/// <summary>
/// Serializes any object and attaches it as a JSON body to the Allure report.
/// </summary>
/// <param name="name">The name of the attachment.</param>
/// <param name="requestModel">The object to serialize and attach.</param>
public static void AttachString<T>(string name, T requestModel) where T : class
{
    AllureApi.Step($"Attach: {name}", () =>
    {
        AllureApi.AddAttachment("Request Body", "application/json", 
            Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(requestModel)));
    });
}
```
3. **Optional Enhancement:** Use the existing JsonHelper instead of JsonConvert for consistency:
```csharp
public static void AttachString<T>(string name, T requestModel) where T : class
{
    AllureApi.Step($"Attach: {name}", () =>
    {
        var jsonContent = JsonHelper.Serialize(requestModel);
        AllureApi.AddAttachment("Request Body", "application/json", 
            Encoding.UTF8.GetBytes(jsonContent));
    });
}
```
4. Update any existing test usage in `Tests/Tests.API/ProductApiTests.cs` to ensure compatibility
5. Test the generic method with different API models:
   - ProductRequest
   - LoginRequest  
   - Any other API model classes

**What you'll learn:**
- Generic methods with type constraints (`where T : class`)
- Code reusability principles
- Method overloading vs. generics
- Consistent use of framework utilities
- Refactoring for maintainability

**Expected Results:**
✅ Method accepts any class object type  
✅ Existing ProductRequest usage continues to work  
✅ New API models can use the same attachment method  
✅ Consistent JSON serialization across the framework

---

## 🎯 Level 4: Advanced Challenges (Intermediate-Advanced)

### Task 4.1: Build a Smart Wait Helper
**Objective:** Create intelligent waiting mechanisms
**Time:** 45 minutes

Create a helper that can wait for complex conditions with custom logic.

**Instructions:**
1. Create `Core/Utils/WaitHelper.cs`
2. Implement flexible waiting:
```csharp
public static class WaitHelper
{
    public static async Task<T> WaitForConditionAsync<T>(
        Func<Task<T>> condition,
        Func<T, bool> predicate,
        TimeSpan timeout,
        TimeSpan pollInterval = default,
        string description = "condition")
    {
        // TODO: Implement polling logic
        // TODO: Check condition every pollInterval
        // TODO: Return result when predicate is true
        // TODO: Throw timeout exception with description
        // TODO: Add logging for debugging
        
        throw new NotImplementedException("Complete this implementation");
    }
    
    // Example usage methods:
    public static async Task<string> WaitForTextToContainAsync(IPage page, string selector, string expectedText, int timeoutMs = 5000)
    {
        // TODO: Use WaitForConditionAsync to wait for text content
        return "";
    }
    
    public static async Task<int> WaitForElementCountAsync(IPage page, string selector, int expectedCount, int timeoutMs = 5000)
    {
        // TODO: Wait for specific number of elements
        return 0;
    }
}
```

**What you'll learn:**
- Generic methods
- Functional programming concepts
- Complex async patterns
- Polling and timeout logic

---

### Task 4.2: Create a Performance Monitor
**Objective:** Learn about performance testing basics
**Time:** 50 minutes

Build a utility to measure and report test performance.

**Instructions:**
1. Create `Core/Utils/PerformanceMonitor.cs`
2. Implement timing and metrics:
```csharp
public class PerformanceMonitor
{
    private readonly Dictionary<string, TimeSpan> _measurements = new();
    private readonly ILogger _logger;
    
    public PerformanceMonitor(ILogger logger)
    {
        _logger = logger;
    }
    
    public async Task<T> MeasureAsync<T>(string operationName, Func<Task<T>> operation)
    {
        // TODO: Start timing
        // TODO: Execute operation
        // TODO: Stop timing and store result
        // TODO: Log performance metrics
        // TODO: Return operation result
        
        throw new NotImplementedException();
    }
    
    public void LogSummary()
    {
        // TODO: Log all measurements
        // TODO: Identify slowest operations
        // TODO: Calculate averages if multiple measurements
    }
    
    // TODO: Add methods for memory usage monitoring
    // TODO: Add methods for page load time measurement
}
```
3. Integrate into your tests to measure critical operations

**What you'll learn:**
- Performance measurement
- Stopwatch and timing
- Metrics collection
- Performance analysis

---

### Task 4.3: Build a Test Report Enhancer
**Objective:** Create custom test reporting and analytics
**Time:** 60 minutes

Enhance the Allure reports with custom data and insights.

**Instructions:**
1. Create `Tests/Helpers/TestReportEnhancer.cs`
2. Add rich reporting features:
```csharp
public static class TestReportEnhancer
{
    public static void AddEnvironmentInfo()
    {
        // TODO: Capture detailed system information
        // TODO: Add browser version, OS details, .NET version
        // TODO: Include test configuration settings
        // TODO: Add timestamp and build information
    }
    
    public static void AddTestStepWithTiming(string stepName, Func<Task> step)
    {
        // TODO: Measure step execution time
        // TODO: Add step to Allure with timing info
        // TODO: Handle step failures gracefully
        // TODO: Add screenshots for UI steps
    }
    
    public static void AddCustomMetrics(Dictionary<string, object> metrics)
    {
        // TODO: Add custom metrics to test report
        // TODO: Format metrics for readability
        // TODO: Include performance thresholds
    }
    
    public static void GenerateTestSummary(TestResult result)
    {
        // TODO: Create detailed test summary
        // TODO: Include execution timeline
        // TODO: Add resource usage information
        // TODO: Generate insights and recommendations
    }
}
```

**What you'll learn:**
- Test reporting concepts
- System information gathering
- Custom Allure attachments
- Data analysis and insights

---

## 🏆 Bonus Challenges

### Bonus 1: Create a Database Seeder
Build a utility that populates the test database with realistic sample data for complex scenarios.

### Bonus 2: Implement Page Object Generator
Create a tool that analyzes a webpage and generates Page Object Model classes automatically.

### Bonus 3: Build a Test Result Analyzer
Create a utility that analyzes test results over time and identifies patterns in failures.

### Bonus 4: Create Mobile Test Support
Extend the framework to support mobile testing with device emulation.

---

## 📝 Workshop Notes

### Tips for Success:
1. **Start small** - Complete Level 1 before moving to harder tasks
2. **Ask questions** - Don't hesitate to ask for help
3. **Test everything** - Run tests after each change
4. **Read error messages** - They contain valuable information
5. **Use debugging** - Set breakpoints to understand code flow

### Common Mistakes to Avoid:
- Forgetting `await` keywords with async methods
- Not handling exceptions properly
- Using hardcoded values instead of configuration
- Skipping input validation
- Not testing edge cases

### Resources:
- [C# Documentation](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [Playwright Documentation](https://playwright.dev/dotnet/)
- [xUnit Documentation](https://xunit.net/)
- [Framework README](../README.md)

---

**Remember:** The goal is to learn programming concepts through practical test automation. Focus on understanding the concepts rather than just completing the tasks!