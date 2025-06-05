using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;

namespace atf.API.Clients
{
    /// <summary>
    /// Base HTTP client wrapper using RestSharp with common request methods.
    /// </summary>
    public abstract class BaseClient : IDisposable
    {
        protected readonly RestClient RestClient;
        private bool _disposed = false;

        protected BaseClient(string baseUrl)
        {
            var options = new RestClientOptions(baseUrl)
            {
                ThrowOnAnyError = false,
                Timeout = TimeSpan.FromSeconds(30)
            };
            RestClient = new RestClient(options);
        }

        protected BaseClient(RestClient restClient)
        {
            RestClient = restClient ?? throw new ArgumentNullException(nameof(restClient));
        }

        protected async Task<T> GetAsync<T>(string uri)
        {
            var request = new RestRequest(uri, Method.Get);

            try
            {
                var response = await RestClient.ExecuteAsync<T>(request);
                return HandleResponse<T>(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during GET {uri}: {ex.Message}");
                throw new HttpRequestException($"Failed to execute GET request to {uri}", ex);
            }
        }

        protected async Task<TResponse> SendAsync<TRequest, TResponse>(Method method, string uri, TRequest content)
            where TRequest : class
        {
            var request = new RestRequest(uri, method);

            if (content != null)
            {
                request.AddJsonBody(content);
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

        protected async Task<RestResponse> SendAsync<TRequest>(Method method, string uri, TRequest content)
            where TRequest : class?
        {
            var request = new RestRequest(uri, method);

            if (content != null)
            {
                request.AddJsonBody(content);
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
            GC.SuppressFinalize(this);
        }
    }
}
