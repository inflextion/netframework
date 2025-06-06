﻿using atf.API.Clients;
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
        protected override string BaseUrl => ConfigManager.Get<string>("TestApiSettings:HttpBinUrl") ?? "https://httpbin.org";

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
