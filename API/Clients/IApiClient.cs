using RestSharp;

namespace atf.API.Clients;

/// <summary>
/// Common interface for all API clients providing standard HTTP operations
/// </summary>
public interface IApiClient : IDisposable
{
    /// <summary>
    /// Performs GET request and deserializes response to specified type
    /// </summary>
    Task<T> GetAsync<T>(string endpoint);
    
    /// <summary>
    /// Performs POST request with payload and deserializes response to specified type
    /// </summary>
    Task<T> PostAsync<T>(object payload, string endpoint);
    
    /// <summary>
    /// Performs PUT request with payload and deserializes response to specified type
    /// </summary>
    Task<T> PutAsync<T>(object payload, string endpoint);
    
    /// <summary>
    /// Performs DELETE request and deserializes response to specified type
    /// </summary>
    Task<T> DeleteAsync<T>(string endpoint);
    
    /// <summary>
    /// Performs GET request and returns raw RestResponse for advanced scenarios
    /// </summary>
    Task<RestResponse> GetRawAsync(string endpoint);
    
    /// <summary>
    /// Performs POST request and returns raw RestResponse for advanced scenarios
    /// </summary>
    Task<RestResponse> PostRawAsync(object payload, string endpoint);
}