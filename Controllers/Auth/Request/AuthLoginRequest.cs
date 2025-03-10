using System.ComponentModel.DataAnnotations;
namespace backend.Controllers.Auth.request;

// nhan du lieu dang nhap tu client
public class AuthLoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}