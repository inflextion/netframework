using atf.Core.Config;
using atf.Core.Enums;
using RestSharp;

namespace atf.API.Clients
{
    /// <summary>
    /// Factory for creating API clients without specifying exact implementation (True Factory Pattern)
    /// </summary>
    public static class ApiClientFactory
    {
        /// <summary>
        /// Creates an API client based on the specified type
        /// </summary>
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

        /// <summary>
        /// Convenience method for creating Product API client
        /// </summary>
        public static IApiClient CreateProductClient()
        {
            return CreateClient(ApiClientType.Product);
        }

        /// <summary>
        /// Convenience method for creating User API client
        /// </summary>
        public static IApiClient CreateUserClient()
        {
            return CreateClient(ApiClientType.User);
        }

        /// <summary>
        /// Creates configured RestClient with default settings
        /// </summary>
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
}