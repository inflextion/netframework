using atf.API.Clients;
using atf.Core.Config;
using RestSharp;
using System;
using System.Net.Http;

namespace atf.API.Fixtures
{
    /// <summary>
    /// Concrete fixture for API tests (for use with IClassFixture).
    /// </summary>
    public class DefaultApiTestFixture : ApiTestFixture
    {
        protected override string BaseUrl => ConfigManager.Get<string>("Playwright:BaseApiHost") ?? "http://localhost:5000";

        protected override BaseClient CreateClient(RestClient restClient)
        {
            return new ProductApiClient(restClient);
        }

        protected override void OnSetup()
        {
            // Simple sync setup
            Console.WriteLine("Simple fixture setup");
        }
    }
}
