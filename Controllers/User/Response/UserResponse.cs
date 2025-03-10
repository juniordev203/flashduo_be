using System.ComponentModel.DataAnnotations;

namespace backend.Controllers.User.Response;

public class UserResponse
{
    public int Id { get; set; }
    [Required] 
    public string fullName { get; set; }
    [Required]
    public string avatarUrl { get; set; }
    public DateTime createdAt { get; set; }
}