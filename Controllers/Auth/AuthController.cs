using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
        var refreshToken = GenerateRefreshToken();
        // Tạo user mới
        var account = new Account
        {
            Email = registerRequest.Email,
            RefreshToken = refreshToken,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.Now
        };

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        var newUser = new Models.User
        {
            AccountId = account.Id,
            FullName = registerRequest.Username,
            AvatarUrl = "",
            CreatedAt = account.CreatedAt
        };
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return Ok("Đăng ký thành công!");
    }

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

        var access_token = GenerateJwtToken(account);
        var refresh_token = GenerateRefreshToken();
        
        account.RefreshToken = refresh_token;
        account.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();
        var response = new AuthLoginResponse
        {
            AccessToken = access_token,
            RefreshToken = refresh_token,
            Id = account.Id,
            Email = account.Email,
            FullName = account.User?.FullName ?? "Unknown",
            Role = account.Role
        };
        return Ok(response);
        
        
    }
    
    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(AuthLoginResponse), 200)]
    public async Task<IActionResult> RefreshToken([FromBody] AuthRefreshTokenRequest refreshTokenRequest)
    {
        if (string.IsNullOrWhiteSpace(refreshTokenRequest.RefreshToken))
        {
            return BadRequest(new { message = "Vui lòng cung cấp Refresh Token." });
        }

        var account = await _context.Accounts
            .FirstOrDefaultAsync(a => a.RefreshToken == refreshTokenRequest.RefreshToken);

        if (account == null || account.RefreshTokenExpiryTime < DateTime.UtcNow)
        {
            return Unauthorized(new { message = "Refresh Token không hợp lệ hoặc đã hết hạn." });
        }

        // Tạo mới Access Token và Refresh Token
        var accessToken = GenerateJwtToken(account);
        var refreshToken = GenerateRefreshToken();

        // Lưu Refresh Token mới vào DB
        account.RefreshToken = refreshToken;
        account.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // Giả sử Refresh Token sống 7 ngày
        await _context.SaveChangesAsync();

        var response = new AuthLoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Id = account.Id,
            Email = account.Email,
            FullName = account.User?.FullName ?? "Unknown",
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
            new Claim("accountId", account.Id.ToString()),
            new Claim(ClaimTypes.Role, account.Role) 
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
    
}