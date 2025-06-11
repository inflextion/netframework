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

namespace atf.Tests.Tests.API
{
    [AllureSuite("Users API")]
    [AllureFeature("User Authentication")]
    public class UserApiTests : UserApiTestBase
    {
        public UserApiTests(ITestOutputHelper output)
            : base(output)
        {
        }

        [Fact(DisplayName = "Should login with valid credentials")]
        [Trait("Category", "API")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureOwner("QA Team")]
        [AllureTag("POST /api/users/login")]
        public async Task LoginUser_WithValidCredentials_ShouldReturnSuccess()
        {
            // Arrange - Using inherited TestLogger with additional context
            var caseLogger = TestLogger.Logger.ForContext("TestMethod", nameof(LoginUser_WithValidCredentials_ShouldReturnSuccess));

            var loginRequest = new LoginRequest
            {
                Username = "testuser",
                Password = "password123"
            };

            caseLogger.Information("Test started: Logging in user with valid credentials");
            caseLogger.Information("Sending POST request to /api/users/login with payload: {@LoginRequest}", loginRequest);
            AllureHelper.AttachString("Request Body", loginRequest.ToString());

            // Act
            var loginResponse = await Client.PostAsync<LoginResponse>(loginRequest, "/api/users/login");

            // Assert
            Assert.NotNull(loginResponse);
            Assert.Equal("Login successful", loginResponse.Message);
            Assert.NotNull(loginResponse.Role);

            AllureHelper.AttachString("Response Body", loginResponse.ToString(), "application/json", ".json");

            caseLogger.Information("User logged in successfully with role: {Role}", loginResponse.Role);
        }

        [Fact(DisplayName = "Should return error for invalid credentials")]
        [Trait("Category", "API")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureOwner("QA Team")]
        [AllureTag("POST /api/users/login")]
        public async Task LoginUser_WithInvalidCredentials_ShouldReturnError()
        {
            // Arrange
            var caseLogger = TestLogger.Logger.ForContext("TestMethod", nameof(LoginUser_WithInvalidCredentials_ShouldReturnError));

            var loginRequest = new LoginRequest
            {
                Username = "invaliduser",
                Password = "wrongpassword"
            };

            caseLogger.Information("Test started: Attempting login with invalid credentials");

            // Act & Assert
            var exception = await Assert.ThrowsAsync<HttpRequestException>(async () =>
                await Client.PostAsync<LoginResponse>(loginRequest, "/api/users/login"));

            Assert.Contains("401", exception.Message);
            caseLogger.Information("Invalid login correctly rejected with 401 error");
        }

        [Fact(DisplayName = "Should retrieve all users")]
        [Trait("Category", "API")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureOwner("QA Team")]
        [AllureTag("GET /api/users")]
        public async Task GetUsers_ShouldReturnUsersList()
        {
            // Arrange
            var caseLogger = TestLogger.Logger.ForContext("TestMethod", nameof(GetUsers_ShouldReturnUsersList));

            caseLogger.Information("Test started: Retrieving all users");

            // Act
            var users = await Client.GetAsync<List<User>>("/api/users");

            // Assert
            Assert.NotNull(users);
            Assert.NotEmpty(users);

            caseLogger.Information("Retrieved {UserCount} users successfully", users.Count);
            AllureHelper.AttachString("Response Body", users.ToString(), "application/json", ".json");
        }
    }
}