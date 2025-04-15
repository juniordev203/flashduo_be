namespace backend.Controllers.Auth.Response;

public class AuthLoginResponse
{
    public string AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public int Id { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string Role { get; set; }
}