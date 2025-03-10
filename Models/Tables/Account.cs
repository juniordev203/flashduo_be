using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Account
{
    [Key]
    public int Id { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required]
    public string PasswordHash { get; set; }
    [Required]
    public string Role { get; set; } = "User";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public User User { get; set; }
    
    public List<ForumPost> Posts { get; set; }
    public List<ForumEmotion> Emotions{ get; set; }
    public List<ForumComment> Comments { get; set; }
}