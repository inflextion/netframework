namespace atf.API.Models;

/// <summary>
/// Response model for successful user login
/// </summary>
public class LoginResponse
{
    public string Message { get; set; }
    public string Role { get; set; }
}