using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class ForumPost
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Content { get; set; }
    [Required]
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    
    [Required]
    public int TopicId { get; set; }
    [ForeignKey("TopicId")]
    public ForumTopic ForumTopic { get; set; }
    
    [Required]
    public int AccountId { get; set; }
    [ForeignKey("AccountId")]
    public Account Account { get; set; }
    
    public List<ForumComment> Comments { get; set; }
    public List<ForumEmotion> Emotions { get; set; }
    
}