using System.Threading.Tasks;
using Moq;
using atf.Tests.Mocks;

namespace atf.Tests.Examples
{
    /// <summary>
    /// Example implementation showing how to use Moq for mocking.
    /// This is for reference only - not an active test.
    /// </summary>
    public class ProductServiceTestExample : BaseMockProvider
    {
        private readonly Mock<IProductRepositoryExample> mockRepo;
        private readonly ProductServiceExample service;

        public ProductServiceTestExample() 
        {
            mockRepo = Mocks.Create<IProductRepositoryExample>(MockBehavior.Strict);
            service = new ProductServiceExample(mockRepo.Object);
        }

        // [Fact] // Uncomment to run as a test
        public async Task GetProductAsync_ReturnsProduct_Example()
        {
            // Arrange: Create a mock for IProductRepository
            var fakeProduct = new ProductExample { Id = 42, Name = "Mocked Product" };
            mockRepo.Setup(r => r.GetByIdAsync(42))
                    .ReturnsAsync(fakeProduct);

            // Act: Call the service method
            var result = await service.GetProductAsync(42);

            // Assert: Verify the returned product
            if (result == null || result.Id != 42 || result.Name != "Mocked Product")
            {
                throw new Exception("Test would fail - unexpected result");
            }
        }
    }

    // Support classes for the example
    internal class ProductServiceExample
    {
        private readonly IProductRepositoryExample _repo;
        
        public ProductServiceExample(IProductRepositoryExample repo)
        {
            _repo = repo;
        }
        
        public async Task<ProductExample> GetProductAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }
    }

    public interface IProductRepositoryExample
    {
        Task<ProductExample> GetByIdAsync(int id);
    }

    public class ProductExample
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
