namespace atf.API.Models
{
    public class ProductRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Category { get; set; } = default!;
        public decimal Price { get; set; }
    }
}
