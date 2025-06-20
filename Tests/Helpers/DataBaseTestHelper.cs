using atf.Core.Utils;
using atf.Data.DatabaseContext;
using atf.Data.Models;
using atf.Data.Repositories;

namespace atf.Tests.Helpers
{
    public static class DatabaseTestHelper
    {
        private static readonly UserRepository _userRepository = new();
        private static readonly ProductRepository _productRepository = new();

        public static void InitializeDatabase()
        {
            DbContextFactory.EnsureDatabaseCreated();
        }

        public static void CleanDatabase()
        {
            DbContextFactory.CleanDatabase();
        }

        public static User CreateTestUser(string name = "Test User", string email = "test@example.com")
        {
            var user = new User
            {
                Name = name,
                Email = email
            };
            return _userRepository.Create(user);
        }

        public static ProductEntity CreateTestProduct(string name = "Test Product", decimal price = 99.99m, bool isActive = true)
        {
            var product = new ProductEntity
            {
                Name = name,
                Price = price,
                IsActive = isActive
            };
            return _productRepository.Create(product);
        }

        /// <summary>
        /// Creates a test user with fake data.
        /// </summary>
        public static User CreateFakeUser()
        {
            var fakeUser = TestDataFaker.CreateFakeUser();
            return _userRepository.Create(fakeUser);
        }

        /// <summary>
        /// Creates a test product with fake data.
        /// </summary>
        public static ProductEntity CreateFakeProduct()
        {
            var fakeProduct = TestDataFaker.CreateFakeProduct();
            return _productRepository.Create(fakeProduct);
        }

        public static User GetUserByEmail(string email)
        {
            return _userRepository.GetByEmail(email);
        }

        public static List<ProductEntity> GetActiveProducts()
        {
            return _productRepository.GetActive();
        }

        public static List<ProductEntity> SearchProducts(string searchTerm)
        {
            return _productRepository.SearchByName(searchTerm);
        }
    }
}
