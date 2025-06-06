using System;
using System.Net;
using Xunit;
using Xunit.Abstractions;
using Allure.Xunit;
using Allure.Xunit.Attributes;
using Serilog;
using atf.API.Builders;
using atf.API.Fixtures;
using atf.API.Models;
using atf.API.Clients;
using atf.Core.Config;
using atf.Tests.Helpers;
using RestSharp;
using Allure.Net.Commons;
using Allure.Xunit.Attributes.Steps;
using atf.Core.Models;
using atf.Core.Utils;

namespace atf.Tests.Tests.API
{
    [AllureSuite("Products API")]
    [AllureFeature("Product CRUD")]
    public class ProductApiTests : BaseApiTest<DefaultApiTestFixture>
    {
        private readonly PlaywrightSettings _settings;
        private readonly ITestOutputHelper _output;

        public ProductApiTests(DefaultApiTestFixture fixture, ITestOutputHelper output)
            : base(fixture, output)
        {
            _settings = ConfigManager.GetSection<PlaywrightSettings>("Playwright");
            _output = output;
        }

        [Fact(DisplayName = "Should create product successfully")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureOwner("QA Team")]
        [AllureTag("POST /api/products")]
        [AllureStep("My Api Test")]
        public async Task PostProduct_ShouldReturnCreated()
        {
            var testName = $"{nameof(PostProduct_ShouldReturnCreated)}_{DateTime.UtcNow:yyyyMMdd_HHmmss}";
            var logFile = Path.Combine("Logs", $"{testName}.log");
            var caseLogger = new LoggerConfiguration()
                .WriteTo.File(logFile,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] [{TestName}] ({TestContext}) ({Application}) {Message:lj}{NewLine}{Exception}")
                .WriteTo.TestOutput(_output, outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] ({TestContext}) ({TestName}) ({Application}) {Message:lj}{NewLine}{Exception}")
                .Enrich.WithProperty("TestName", testName)
                .Enrich.WithProperty("Application", "AutomationFramework")
                .Enrich.WithProperty("TestContext", "XUnit")
                .CreateLogger();

            caseLogger.Information("Test started: Creating a new product");

            var productRequest = new ProductRequestBuilder()
                .WithFakeData()
                .Build();

            caseLogger.Information("Sending POST request to /api/products with payload: {@ProductRequest}", productRequest);
            AllureHelper.AttachString("Request Body", productRequest);

            // SOLUTION: Use the public CreateProductAsync method
            // If Client is already a ProductApiClient, cast it
            var productClient = Client as ProductApiClient;
            if (productClient == null)
            {
                throw new InvalidOperationException("Client is not a ProductApiClient instance");
            }
            
            var createdProduct = await productClient.CreateProductAsync(productRequest, isTestRequest: true);

            caseLogger.Information("Product created successfully");

            // Validate the response
            Assert.NotNull(createdProduct);
            Assert.Equal(productRequest.Name, createdProduct.Name);
            Assert.Equal(productRequest.Category, createdProduct.Category);
            Assert.Equal(productRequest.Price, createdProduct.Price);

            AllureHelper.AttachString("Response Body", createdProduct.ToString(), "application/json", ".json");

            caseLogger.Information("Product created and validated successfully. Test finished.");
        }

        [Fact(DisplayName = "Should create product with mixed faker and real data")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureOwner("QA Team")]
        [AllureTag("POST /api/products")]
        public async Task PostProduct_WithMixedFakeData_ShouldReturnCreated()
        {
            // Arrange - Mix of fake and specific data
            var productRequest = new ProductRequestBuilder()
                .WithFakeId()           // Random ID
                .WithFakeName()         // Random product name
                .WithCategory("Laptops") // Specific category
                .WithFakePrice(100, 500) // Random price in range
                .Build();

            Logger.Information("Creating product with mixed fake data: {@ProductRequest}", productRequest);

            // Act
            var productClient = Client as ProductApiClient;
            var createdProduct = await productClient!.CreateProductAsync(productRequest, isTestRequest: true);

            // Assert
            Assert.NotNull(createdProduct);
            Assert.Equal(productRequest.Name, createdProduct.Name);
            Assert.Equal("Laptops", createdProduct.Category); // Verify specific category
            Assert.True(createdProduct.Price >= 100 && createdProduct.Price <= 500); // Verify price range

            Logger.Information("Product with mixed fake data created successfully");
        }
    }
}