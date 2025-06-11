using Xunit.Abstractions;
using atf.API.Clients;

namespace atf.Tests.Base
{
    /// <summary>
    /// Simplified base class for Product API tests.
    /// </summary>
    public abstract class ProductApiTestBase : BaseApiTest<IApiClient>
    {
        protected ProductApiTestBase(ITestOutputHelper output) : base(output) { }

        protected override IApiClient CreateClient()
        {
            return ApiClientFactory.CreateProductClient();
        }
    }
}