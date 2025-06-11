using atf.API.Models;
using atf.Core.Utils;

namespace atf.API.Builders
{
    public class ProductRequestBuilder
    {
        private int _id = 1;
        private string _name = "Sample Product";
        private string _category = "Laptops";
        private decimal _price = 999.99M;

        public ProductRequestBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public ProductRequestBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ProductRequestBuilder WithCategory(string category)
        {
            _category = category;
            return this;
        }

        public ProductRequestBuilder WithPrice(decimal price)
        {
            _price = price;
            return this;
        }

        /// <summary>
        /// Uses faker to generate random test data for all fields.
        /// </summary>
        public ProductRequestBuilder WithFakeData()
        {
            var fakeProduct = TestDataFaker.CreateFakeProductRequest();
            _id = fakeProduct.Id;
            _name = fakeProduct.Name;
            _category = fakeProduct.Category;
            _price = fakeProduct.Price;
            return this;
        }

        /// <summary>
        /// Generates a random ID using faker.
        /// </summary>
        public ProductRequestBuilder WithFakeId()
        {
            _id = TestDataFaker.FakeId();
            return this;
        }

        /// <summary>
        /// Generates a random product name using faker.
        /// </summary>
        public ProductRequestBuilder WithFakeName()
        {
            _name = TestDataFaker.FakeProductName();
            return this;
        }

        /// <summary>
        /// Generates a random category using faker.
        /// </summary>
        public ProductRequestBuilder WithFakeCategory()
        {
            _category = TestDataFaker.FakeCategory();
            return this;
        }

        /// <summary>
        /// Generates a random price using faker.
        /// </summary>
        public ProductRequestBuilder WithFakePrice(decimal min = 1, decimal max = 2000)
        {
            _price = TestDataFaker.FakePrice(min, max);
            return this;
        }

        public ProductRequest Build()
        {
            return new ProductRequest
            {
                Id = _id,
                Name = _name,
                Category = _category,
                Price = _price
            };
        }
    }

}
