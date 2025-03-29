using System.Security.Claims;
using backend.Controllers.User.Response;
using backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace backend.Controllers.User;

[ApiExplorerSettings(GroupName = "User")]
[Tags("User")]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    public readonly AppDbContext _context;
    public readonly IConfiguration _config;

    public UserController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _config = configuration;
    }
    [HttpGet("info-user")]
    [ProducesResponseType(typeof(UserResponse), 200)]
    public async Task<IActionResult> GetUserInfo(int accountId)
    {
        try
        {
            if (accountId <= 0) return BadRequest("loi account id");
            // Tìm user theo accountId
            var user = await _context.Users
                .Where(u => u.AccountId == accountId)
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    fullName = u.FullName,
                    avatarUrl = u.AvatarUrl,
                }).FirstOrDefaultAsync();
            if (user == null) return NotFound("Khong tim thay nguoi dung");
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Lỗi server: {ex.Message}");
        }
    }
    
}