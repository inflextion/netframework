using System.Text.Json.Serialization;
using Azure.Core.Serialization;

namespace atf.API.Models
{
    public class ProductRequest : Product
    {
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }
}
