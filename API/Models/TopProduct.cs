using System.Text.Json.Serialization;

namespace atf.API.Models
{
    public class TopProduct : Product
    {
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }
}