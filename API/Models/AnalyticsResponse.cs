using System.Text.Json.Serialization;

namespace atf.API.Models
{
    public class AnalyticsResponse
    {
        [JsonPropertyName("totalRevenue")]
        public decimal TotalRevenue { get; set; }

        [JsonPropertyName("topProducts")]
        public List<TopProduct> TopProducts { get; set; } = new();

        [JsonPropertyName("salesByCategory")]
        public Dictionary<string, int> SalesByCategory { get; set; } = new();

        [JsonPropertyName("mergedProducts")]
        public List<MergedProduct> MergedProducts { get; set; } = new();
    }
}