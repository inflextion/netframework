namespace atf.API.Models;

/// <summary>
/// Request model for user login authentication
/// </summary>
public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
}