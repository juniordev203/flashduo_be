using System.ComponentModel.DataAnnotations;

namespace backend.Controllers.Auth.request;

public class AuthRegisterRequest
{
    [Required]
    public string Username { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, MinLength(6)]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "Xác nhận mật khẩu không khớp!")]
    public string FirmPassword { get; set; }
    
}
