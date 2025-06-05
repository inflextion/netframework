using atf.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
