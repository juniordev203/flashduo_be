using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class ForumTopic
{
    [Key] 
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public int CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public ForumCategory ForumCategory { get; set; }
    
    public List<ForumPost> Posts { get; set; }
}