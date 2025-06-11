using atf.API.Clients;
using atf.Core.Config;
using RestSharp;
using System.Threading.Tasks;
using Xunit;

namespace atf.API.Fixtures
{
    /// <summary>
    /// xUnit fixture for API tests setting up RestSharp RestClient.
    /// </summary>
    public abstract class ApiTestFixture : IAsyncLifetime, IDisposable
    {
        public BaseClient Client { get; private set; }
        protected RestClient RestClient { get; private set; }

        protected virtual string BaseUrl => ConfigManager.Get<string>("TestApiSettings:BaseUrl") ?? "https://jsonplaceholder.typicode.com";
        protected virtual TimeSpan Timeout => TimeSpan.FromMilliseconds(ConfigManager.Get<int>("ApiClient:TimeoutMs"));

        public async Task InitializeAsync()
        {
            
            var options = new RestClientOptions(BaseUrl)
            {
                ThrowOnAnyError = false,
                Timeout = Timeout
            };

            RestClient = new RestClient(options);
            Client = CreateClient(RestClient);

            await OnSetupAsync();
        }

        protected abstract BaseClient CreateClient(RestClient restClient);

        protected virtual Task OnSetupAsync()
        {
            OnSetup();
            return Task.CompletedTask;
        }

        protected virtual void OnSetup() { }

        public async Task DisposeAsync()
        {
            await OnTeardownAsync();

            Client?.Dispose();
            RestClient?.Dispose();
        }

        protected virtual Task OnTeardownAsync()
        {
            OnTeardown();
            return Task.CompletedTask;
        }

        protected virtual void OnTeardown() { }

        public void Dispose()
        {
            DisposeAsync().GetAwaiter().GetResult();
        }
    }
}