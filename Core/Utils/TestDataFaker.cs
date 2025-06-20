using Bogus;
using atf.API.Models;
using atf.Data.Models;

namespace atf.Core.Utils
{
    /// <summary>
    /// Simple utility class for generating fake test data using Bogus library.
    /// </summary>
    public static class TestDataFaker
    {
        private static readonly Faker _faker = new Faker();

        /// <summary>
        /// Generates a fake product for testing.
        /// </summary>
        /// <returns>Product with fake data</returns>
        public static ProductEntity CreateFakeProduct()
        {
            var categories = new[] { "Laptops", "Phones", "Accessories", "Tablets" };
            
            return new ProductEntity
            {
                Name = _faker.Commerce.ProductName(),
                Price = decimal.Parse(_faker.Commerce.Price(10, 2000)),
                IsActive = _faker.Random.Bool(0.8f) // 80% chance of being active
            };
        }

        /// <summary>
        /// Generates a fake product request for API testing.
        /// </summary>
        /// <returns>ProductRequest with fake data</returns>
        public static ProductRequest CreateFakeProductRequest()
        {
            var categories = new[] { "Laptops", "Phones", "Accessories", "Tablets" };
            
            return new ProductRequest
            {
                Id = _faker.Random.Int(10000, 99999),
                Name = _faker.Commerce.ProductName(),
                Category = _faker.PickRandom(categories),
                Price = decimal.Parse(_faker.Commerce.Price(10, 2000))
            };
        }
        
        /// <summary>
        /// Creates a fake product name.
        /// </summary>
        /// <returns>Random product name</returns>
        public static string FakeProductName() => _faker.Commerce.ProductName();

        /// <summary>
        /// Creates a fake price between specified range.
        /// </summary>
        /// <param name="min">Minimum price</param>
        /// <param name="max">Maximum price</param>
        /// <returns>Random price as decimal</returns>
        public static decimal FakePrice(decimal min = 1, decimal max = 2000) => 
            decimal.Parse(_faker.Commerce.Price(min, max));

        /// <summary>
        /// Picks a random category from predefined list.
        /// </summary>
        /// <returns>Random category</returns>
        public static string FakeCategory()
        {
            var categories = new[] { "Laptops", "Phones", "Accessories", "Tablets", "Gaming", "Software" };
            return _faker.PickRandom(categories);
        }

        /// <summary>
        /// Creates a random ID in test range.
        /// </summary>
        /// <returns>Random ID between 10000-99999</returns>
        public static int FakeId() => _faker.Random.Int(10000, 99999);

        /// <summary>
        /// Creates fake text for descriptions, etc.
        /// </summary>
        /// <param name="sentences">Number of sentences</param>
        /// <returns>Random text</returns>
        public static string FakeText(int sentences = 2) => _faker.Lorem.Sentences(sentences);

        /// <summary>
        /// Generates a fake user for testing.
        /// </summary>
        /// <returns>User with fake data</returns>
        public static User CreateFakeUser()
        {
            return new User
            {
                Name = _faker.Name.FullName(),
                Email = _faker.Internet.Email()
            };
        }
        
        /// <summary>
        /// Creates a fake email address.
        /// </summary>
        /// <returns>Random email address</returns>
        public static string FakeEmail() => _faker.Internet.Email();

        /// <summary>
        /// Creates a fake full name.
        /// </summary>
        /// <returns>Random full name</returns>
        public static string FakeName() => _faker.Name.FullName();
    }
}