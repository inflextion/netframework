using atf.API.Clients;
using atf.Core.Logging;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace atf.Tests.Base
{
    /// <summary>
    /// Base class for API tests with simplified client creation and proper lifecycle management.
    /// </summary>
    public abstract class BaseApiTest<TClient> : IAsyncLifetime, IDisposable
        where TClient : IApiClient
    {
        protected TClient Client { get; private set; }
        protected ITestOutputHelper OutputHelper { get; private set; }
        protected TestLogger TestLogger { get; private set; }

        protected BaseApiTest(ITestOutputHelper output)
        {
            OutputHelper = output;
            TestLogger = new TestLogger(output, GetType().Name, writeToFile: true);
        }

        protected abstract TClient CreateClient();

        // IAsyncLifetime implementation
        public virtual async Task InitializeAsync()
        {
            Client = CreateClient();
            await OnSetupAsync();
        }

        public virtual async Task DisposeAsync()
        {
            await OnTeardownAsync();
            Client?.Dispose();
            TestLogger?.Dispose();
        }

        // Template methods for setup/teardown
        protected virtual Task OnSetupAsync()
        {
            OnSetup();
            return Task.CompletedTask;
        }

        protected virtual void OnSetup() { }

        protected virtual Task OnTeardownAsync()
        {
            OnTeardown();
            return Task.CompletedTask;
        }

        protected virtual void OnTeardown() { }

        // IDisposable implementation
        public virtual void Dispose()
        {
            DisposeAsync().GetAwaiter().GetResult();
        }
    }
}