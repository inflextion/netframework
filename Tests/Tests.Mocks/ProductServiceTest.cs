//using System.Threading.Tasks;
//using Moq;
//using Xunit;
//using atf.Tests.Mocks;
//using atf.Data.Repositories;

//public class ProductServiceTests : BaseMockProvider
//{
//    private readonly Mock<IProductRepository> mockRepo;
//    private readonly ProductService service;

//    public ProductServiceTests() 
//    {
//        mockRepo = Mocks.Create<IProductRepository>(MockBehavior.Strict);
//        service = new ProductService(mockRepo.Object);
//    }


//    [Fact]
//    public async Task GetProductAsync_ReturnsProduct()
//    {
//        // Arrange: Create a mock for IProductRepository

//        // Set up the mock: When GetByIdAsync(42) is called, return a fake product
//        var fakeProduct = new Products { Id = 42, Name = "Mocked Product" };
//        mockRepo.Setup(r => r.GetByIdAsync(42))
//                .ReturnsAsync(fakeProduct);

//        // Pass the mock to your service

//        // Act: Call the service method
//        var result = await service.GetProductAsync(42);

//        // Assert: The returned product is the one you set up
//        Assert.NotNull(result);
//        Assert.Equal(42, result.Id);
//        Assert.Equal("Mocked Product", result.Name);
//    }
//}


////support classes 
//internal class ProductService
//{
//    private readonly IProductRepository _repo;
//    public ProductService(IProductRepository repo)
//    {
//        _repo = repo;
//    }
//    public async Task<Products> GetProductAsync(int id)
//    {
//        return await _repo.GetByIdAsync(id);
//    }
//}

//public interface IProductRepository
//{
//    Task<Products> GetByIdAsync(int id);
//}

//public class Products
//{
//    public int Id { get; set; }
//    public string Name { get; set; }
//}
