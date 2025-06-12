using RestSharp;
using System.Threading.Tasks;
using System.Collections.Generic;
using atf.API.Models;

namespace atf.API.Clients
{
    /// <summary>
    /// Specific API client for Product operations
    /// </summary>
    public class ProductApiClient : BaseClient
    {

        public ProductApiClient(RestClient restClient) : base(restClient) { }

        public async Task<ProductRequest> CreateProductAsync(ProductRequest product, bool isTestRequest = false)
        {
            var headers = new Dictionary<string, string>();
            
            // Add custom header for test requests if needed
            if (isTestRequest)
            {
                headers.Add("X-Test-Request", "true");
            }

            return await SendAsync<ProductRequest, ProductRequest>(Method.Post, "/api/products", product, headers);
        }

        public async Task<ProductRequest> GetProductAsync(int productId)
        {
            return await GetAsync<ProductRequest>($"/api/products/{productId}");
        }

        public async Task<List<ProductRequest>> GetProductsAsync()
        {
            return await GetAsync<List<ProductRequest>>("/api/products");
        }

        public async Task<ProductRequest> UpdateProductAsync(int productId, ProductRequest product)
        {
            return await SendAsync<ProductRequest, ProductRequest>(Method.Put, $"/api/products/{productId}", product);
        }

        public async Task DeleteProductAsync(int productId)
        {
            await SendAsync(Method.Delete, $"/api/products/{productId}", (object)null, headers: null);
        }

        public async Task<List<ProductRequest>> GetProductsByCategoryAsync(string category)
        {
            return await GetAsync<List<ProductRequest>>($"/api/products?category={category}");
        }
    }
}