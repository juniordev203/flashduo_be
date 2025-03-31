using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backend.Controllers.Auth.request;
using backend.Controllers.Auth.Response;
using backend.Controllers.Response;
using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Controllers.Auth;
[ApiExplorerSettings(GroupName = "Auth")] // Nhóm API này vào "Auth"
[Tags("Auth")] 
[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;
    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _config = configuration;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthRegisterRequest registerRequest)
    {
        if (registerRequest.Password != registerRequest.FirmPassword)
        {
            return BadRequest("Mật khẩu không khớp");
        }
        if (await _context.Accounts.AnyAsync(a => a.Email == registerRequest.Email))
        {
            return BadRequest("Email đã tồn tại");
        }

        if (string.IsNullOrEmpty(registerRequest.Email))
        {
            return BadRequest(new { message = "Email không được để trống!" });
        }
        // ma hoa mat khau
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

        // Tạo user mới
        var account = new Account
        {
            Email = registerRequest.Email,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.Now
        };

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        var newUser = new Models.User
        {
            AccountId = account.Id,
            FullName = registerRequest.Username,
            AvatarUrl = "https://www.google.com/url?sa=i&url=https%3A%2F%2Fwww.istockphoto.com%2Fphotos%2Fuser-avatar&psig=AOvVaw2cN4bWSN2jFoSNIxHrNrXl&ust=1741756548332000&source=images&cd=vfe&opi=89978449&ved=0CBEQjRxqFwoTCICJ8_SigYwDFQAAAAAdAAAAABAE",
            CreatedAt = account.CreatedAt
        };
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return Ok("Đăng ký thành công!");
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthLoginResponse), 200)]
    public async Task<IActionResult> Login([FromBody] AuthLoginRequest loginRequest)
    {
        if (string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Password))
        {
            return BadRequest(new { message = "Vui lòng điền email và mật khẩu!" });
        }

        if (!IsValidEmail(loginRequest.Email))
        {
            return BadRequest(new { message ="email không hợp lệ! "});
        }
        var account = await _context.Accounts
            .Include(a => a.User)
            .FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
        if (account == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, account.PasswordHash))
        {
            return Unauthorized(new { message = "Tài khoản hoặc mật khẩu không đúng." });
        }

        var token = GenerateJwtToken(account);
        var response = new AuthLoginResponse
        {
            Token = token,
            Id = account.Id,
            Email = account.Email,
            FullName = "Flashduo Guy",
            Role = account.Role
        };
        return Ok(response);
        
        
    }
    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private string GenerateJwtToken(Account account)
    {
        var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, account.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, account.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("accountId", account.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(12),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}