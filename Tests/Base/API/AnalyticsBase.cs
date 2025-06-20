using Xunit.Abstractions;
using atf.API.Clients;

namespace atf.Tests.Base
{
    /// <summary>
    /// Base class for User API tests with authentication scenarios
    /// </summary>
    public abstract class AnalyticsBase : BaseApiTest<IApiClient>
    {
        protected AnalyticsBase(ITestOutputHelper output) : base(output) { }

        protected override IApiClient CreateClient()
        {
            return ApiClientFactory.CreateAnalyticsClient();
        }
    }
}