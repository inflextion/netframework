using System;
using System.Net;
using Xunit;
using Xunit.Abstractions;
using Allure.Xunit;
using Allure.Xunit.Attributes;
using atf.API.Models;
using atf.API.Clients;
using atf.Tests.Helpers;
using atf.Tests.Base;
using Allure.Net.Commons;
using atf.Data.Models;
using Newtonsoft.Json.Linq;

namespace atf.Tests.Tests.API
{
    [AllureSuite("Analytics API")]
    [AllureFeature("Get Analytics Data")]
    public class AnalyticsTest : AnalyticsBase
    {
        public AnalyticsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact(DisplayName = "Should return analytics data using RAW response")]
        [Trait("Category", "API")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureOwner("QA Team")]
        [AllureTag("GET /api/analytics")]
        public async Task GetAnalytics_GetRawAsync()
        {
            // Arrange - Using inherited TestLogger with additional context
            var caseLogger = TestLogger.ForContext("TestMethod", nameof(GetAnalytics_GetRawAsync));

            caseLogger.Information("Test started");
            caseLogger.Information("Sending GET  request to /api/analytics");

            // Act
            // Using GetRawAsync to retrieve raw response data and deserialize later with JsonHelper
            var analyticsData = await Client.GetRawAsync("/api/analytics");
            caseLogger.Information("Analytics returned successful: {AnalyticsResponse}", analyticsData.Content);
            AllureHelper.AttachString("Response Body", analyticsData.Content, "application/json", ".json");


            JObject analyticsJson = JsonHelper.Deserialize<JObject>(analyticsData.Content);
            
            AllureHelper.AttachString("Response Body", analyticsJson.ToString(), "application/json", ".json");

            JToken topProducts = analyticsJson["topProducts"];
            //1st option: Deserialize directly to a list of TopProduct
            var topProductsList = topProducts.ToObject<List<TopProduct>>();

            //2nd option: Deserialize to a custom AnalyticsResponse model
            var analytics = JsonHelper.Deserialize<AnalyticsResponse>(analyticsData.Content);

            //Assert
            // LINQ Examples with Analytics Data
            // 1. Top product by quantity
            var topProduct = analytics.TopProducts.OrderByDescending(p => p.Quantity).First();
            caseLogger.Information($"Top Product: {topProduct.Name} (Quantity: {topProduct.Quantity})");

        }

        [Fact(DisplayName = "Should return analytics data using strongly typed model")]
        public async Task GetAnalytics_GetTypedAsync()
        {
            // Arrange
            var caseLogger = TestLogger.ForContext("TestMethod", nameof(GetAnalytics_GetTypedAsync));

            caseLogger.Information("Test started");
            caseLogger.Information("Sending GET request to /api/analytics");

            // Act
            var analyticsData = await Client.GetAsync<AnalyticsResponse>("/api/analytics");
            caseLogger.Information("Analytics returned successful: {AnalyticsResponse}", analyticsData);
            AllureHelper.AttachString("Response Body", JsonHelper.Serialize(analyticsData), "application/json", ".json");

            // Assert
            Assert.NotNull(analyticsData);
            Assert.NotEmpty(analyticsData.TopProducts);

            // Example of LINQ usage with strongly typed model
            var topProduct = analyticsData.TopProducts.OrderByDescending(p => p.Quantity).First();
            caseLogger.Information($"Top Product: {topProduct.Name} (Quantity: {topProduct.Quantity})");
        }
    }
}