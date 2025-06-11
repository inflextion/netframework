using atf.Core.Config;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;

namespace atf.API.Clients
{
    /// <summary>
    /// Base HTTP client wrapper using RestSharp with common request methods.
    /// </summary>
    public abstract class BaseClient : IApiClient
    {
        protected readonly RestClient RestClient;
        private bool _disposed = false;
        
        protected BaseClient(RestClient restClient)
        {
            RestClient = restClient ?? throw new ArgumentNullException(nameof(restClient));
        }

        public virtual async Task<T> GetAsync<T>(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.Get);

            try
            {
                var response = await RestClient.ExecuteAsync<T>(request);
                return HandleResponse<T>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during GET {endpoint}: {ex.Message}");
                throw new HttpRequestException($"Failed to execute GET request to {endpoint}", ex);
            }
        }

        public virtual async Task<T> PostAsync<T>(object payload, string endpoint)
        {
            return await SendAsync<object, T>(Method.Post, endpoint, payload);
        }

        public virtual async Task<T> PutAsync<T>(object payload, string endpoint)
        {
            return await SendAsync<object, T>(Method.Put, endpoint, payload);
        }

        public virtual async Task<T> DeleteAsync<T>(string endpoint)
        {
            return await SendAsync<object, T>(Method.Delete, endpoint, null);
        }

        public virtual async Task<RestResponse> GetRawAsync(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.Get);
            return await RestClient.ExecuteAsync(request);
        }

        public virtual async Task<RestResponse> PostRawAsync(object payload, string endpoint)
        {
            return await SendAsync(Method.Post, endpoint, payload, null);
        }

        /// <summary>
        /// Sends an HTTP request with a JSON body and deserializes the response to the specified type.
        /// Use this when you expect a strongly-typed response from the API.
        /// </summary>
        protected async Task<TResponse> SendAsync<TRequest, TResponse>(Method method, string uri, TRequest content, Dictionary<string, string> headers = null)
            where TRequest : class
        {
            var request = new RestRequest(uri, method);

            if (content != null)
            {
                request.AddJsonBody(content);
            }
            
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }

            try
            {
                var response = await RestClient.ExecuteAsync<TResponse>(request);
                return HandleResponse<TResponse>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during {method} {uri}: {ex.Message}");
                throw new HttpRequestException($"Failed to execute {method} request to {uri}", ex);
            }
        }

        /// <summary>
        /// Sends an HTTP request with a JSON body and returns the raw RestResponse.
        /// Use this when you need access to the full response details (status code, headers, etc.) or do not require deserialization.
        /// </summary>
        protected async Task<RestResponse> SendAsync<TRequest>(Method method, string uri, TRequest content, Dictionary<string, string> headers)
            where TRequest : class
        {
            var request = new RestRequest(uri, method);

            if (content != null)
            {
                request.AddJsonBody(content);
            }

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }

            try
            {
                var response = await RestClient.ExecuteAsync(request);

                if (!response.IsSuccessful)
                {
                    throw new HttpRequestException($"Request failed with status {response.StatusCode}: {response.ErrorMessage ?? response.Content}");
                }

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during {method} {uri}: {ex.Message}");
                throw new HttpRequestException($"Failed to execute {method} request to {uri}", ex);
            }
        }

        private T HandleResponse<T>(RestResponse<T> response)
        {
            if (response == null)
            {
                throw new HttpRequestException("Response is null");
            }

            if (!response.IsSuccessful)
            {
                var errorMessage = response.ErrorMessage ?? response.Content ?? "Unknown error";

                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        throw new HttpRequestException($"Resource not found (404): {errorMessage}");
                    case HttpStatusCode.Unauthorized:
                        throw new UnauthorizedAccessException($"Unauthorized (401): {errorMessage}");
                    case HttpStatusCode.Forbidden:
                        throw new UnauthorizedAccessException($"Forbidden (403): {errorMessage}");
                    case HttpStatusCode.BadRequest:
                        throw new ArgumentException($"Bad request (400): {errorMessage}");
                    case HttpStatusCode.InternalServerError:
                        throw new HttpRequestException($"Server error (500): {errorMessage}");
                    default:
                        throw new HttpRequestException($"Request failed ({(int)response.StatusCode}): {errorMessage}");
                }
            }

            if (response.Data == null && typeof(T) != typeof(object))
            {
                throw new InvalidOperationException($"Response deserialization failed for type {typeof(T).Name}");
            }

            return response.Data;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                RestClient?.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
