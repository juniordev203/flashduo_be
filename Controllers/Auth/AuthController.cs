using backend.Controllers.Auth.request;
using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;

namespace backend.Controllers.Auth;

[Route("api/auth")]
[ApiController]
public class AuthController : Controller
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthRequestRegister request)
    {
        if (request.Password != request.FirmPassword)
        {
            return BadRequest("Mật khẩu không khớp");
        }
        if (await _context.Users.AnyAsync(u => u.Username == request.Username))
        {
            return BadRequest("Ten tai khoan da ton tai");
        }
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // Tạo user mới
        var user = new User
        {
            Username = request.Username,
            PasswordHash = passwordHash
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Đăng ký thành công!");
    }
}