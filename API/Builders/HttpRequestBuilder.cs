using Newtonsoft.Json;
using System.Text;


namespace Automation.API.Builders
{
    /// <summary>
    /// Builds HttpRequestMessage instances with JSON bodies and optional headers.
    /// </summary>
    public class HttpRequestBuilder
    {
        private HttpMethod _method = HttpMethod.Get;
        private string _uri = string.Empty;
        private object _body;
        private readonly Dictionary<string, string> _headers = new();

        public HttpRequestBuilder WithMethod(HttpMethod method)
        {
            _method = method;
            return this;
        }

        public HttpRequestBuilder WithUri(string uri)
        {
            _uri = uri;
            return this;
        }

        public HttpRequestBuilder WithJsonBody<T>(T body)
        {
            _body = body;
            return this;
        }

        public HttpRequestBuilder AddHeader(string name, string value)
        {
            _headers[name] = value;
            return this;
        }

        public HttpRequestMessage Build()
        {
            if (string.IsNullOrWhiteSpace(_uri))
                throw new InvalidOperationException("Request URI must be set.");

            var request = new HttpRequestMessage(_method, _uri);

            if (_body is not null)
            {
                var json = JsonConvert.SerializeObject(_body);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            foreach (var (name, value) in _headers)
                request.Headers.Add(name, value);

            return request;
        }
    }
}
/*
 * 
 * 
 * var httpRequest = new HttpRequestBuilder()
    .WithMethod(HttpMethod.Post)
    .WithUri("/api/users")
    .WithJsonBody(new CreateUserRequest { 
        Username = "john.doe", 
        Email = "john@example.com", 
        Password = "S3cr3t!" 
    })
    .AddHeader("X-Request-Id", Guid.NewGuid().ToString())
    .Build();

var user = await apiClient.SendAsync<CreateUserRequest, CreateUserResponse>(
    httpRequest.Method,
    httpRequest.RequestUri.ToString(),
    ((dynamic)httpRequest.Content!) // or adjust your SendAsync overload
);


*/