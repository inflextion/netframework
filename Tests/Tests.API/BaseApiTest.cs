using RestSharp;
using Serilog;
using Xunit;
using Xunit.Abstractions;
using atf.API.Clients;
using atf.API.Fixtures;

namespace atf.Tests.Tests.API
{
    /// <summary>
    /// Base class for API tests using RestSharp BaseClient.
    /// </summary>
    public abstract class BaseApiTest<TFixture> : IClassFixture<TFixture>
        where TFixture : ApiTestFixture
    {
        protected ProductApiClient Client { get; private set; }
        protected ITestOutputHelper OutputHelper { get; private set; }
        protected ILogger Logger { get; private set; }

        protected BaseApiTest(TFixture fixture, ITestOutputHelper output)
        {
            Client = (ProductApiClient)fixture.Client;
            OutputHelper = output;

            Logger = new LoggerConfiguration()
                .WriteTo.TestOutput(output, outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] ({Application}) {Message:lj}{NewLine}{Exception}")
                .Enrich.WithProperty("Application", "AutomationFramework")
                .Enrich.WithProperty("TestContext", "XUnit")
                .CreateLogger();
        }
    }
}