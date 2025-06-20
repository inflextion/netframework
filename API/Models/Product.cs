using System.Text.Json.Serialization;

namespace atf.API.Models
{
    public class Product
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("category")]
        public string Category { get; set; } = default!;
    }
}