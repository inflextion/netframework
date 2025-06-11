using atf.API.Models;
using atf.Data.Models;
using RestSharp;

namespace atf.API.Clients;

/// <summary>
/// API client for user management and authentication operations
/// </summary>
public class UserApiClient : BaseClient
{
    public UserApiClient(RestClient restClient) : base(restClient)
    {
    }

    /// <summary>
    /// Authenticates user with username and password
    /// </summary>
    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        return await PostAsync<LoginResponse>(loginRequest, "/api/users/login");
    }

    /// <summary>
    /// Retrieves list of all users
    /// </summary>
    public async Task<List<User>> GetUsersAsync()
    {
        return await GetAsync<List<User>>("/api/users");
    }
}