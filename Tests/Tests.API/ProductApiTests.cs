using System;
using System.Net;
using Xunit;
using Xunit.Abstractions;
using Allure.Xunit;
using Allure.Xunit.Attributes;
using Serilog;
using atf.API.Builders;
using atf.API.Models;
using atf.API.Clients;
using atf.Core.Config;
using atf.Tests.Helpers;
using atf.Tests.Base;
using RestSharp;
using Allure.Net.Commons;
using Allure.Xunit.Attributes.Steps;
using atf.Core.Models;
using atf.Core.Utils;

namespace atf.Tests.Tests.API
{
    [AllureSuite("Products API")]
    [AllureFeature("Product CRUD")]
    public class ProductApiTests : ProductApiTestBase
    {
        private readonly PlaywrightSettings _settings;
        private readonly ITestOutputHelper _output;

        public ProductApiTests(ITestOutputHelper output)
            : base(output)
        {
            _settings = ConfigManager.GetSection<PlaywrightSettings>("Playwright");
            _output = output;
        }

        [Fact(DisplayName = "Should create product successfully")]
        [Trait("Category", "API")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureOwner("QA Team")]
        [AllureTag("POST /api/products")]
        [AllureStep("My Api Test")]
        public async Task PostProduct_ShouldReturnCreated()
        {
            // Use inherited TestLogger with additional context
            var caseLogger = TestLogger.Logger.ForContext("TestMethod", nameof(PostProduct_ShouldReturnCreated));

            caseLogger.Information("Test started: Creating a new product");

            var productRequest = new ProductRequestBuilder()
                .WithFakeData()
                .Build();

            caseLogger.Information("Sending POST request to /api/products with payload: {@ProductRequest}", productRequest);
            AllureHelper.AttachString("Request Body", productRequest);

            var createdProduct = await Client.PostAsync<ProductRequest>(productRequest, "/api/products");

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
        [Trait("Category", "API")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureOwner("QA Team")]
        [AllureTag("POST /api/products")]
        public async Task PostProduct_WithMixedFakeData_ShouldReturnCreated()
        {
            // Use inherited TestLogger with additional context
            var caseLogger = TestLogger.Logger.ForContext("TestMethod", nameof(PostProduct_WithMixedFakeData_ShouldReturnCreated));
            
            // Arrange - Mix of fake and specific data
            var productRequest = new ProductRequestBuilder()
                .WithFakeId()           // Random ID
                .WithFakeName()         // Random product name
                .WithCategory("Laptops") // Specific category
                .WithFakePrice(100, 500) // Random price in range
                .Build();

            caseLogger.Information("Creating product with mixed fake data: {@ProductRequest}", productRequest);

            // Act
            var createdProduct = await Client.PostAsync<ProductRequest>(productRequest, "/api/products");

            // Assert
            Assert.NotNull(createdProduct);
            Assert.Equal(productRequest.Name, createdProduct.Name);
            Assert.Equal("Laptops", createdProduct.Category); // Verify specific category
            Assert.True(createdProduct.Price >= 100 && createdProduct.Price <= 500); // Verify price range

            caseLogger.Information("Product with mixed fake data created successfully");
        }
    }
}