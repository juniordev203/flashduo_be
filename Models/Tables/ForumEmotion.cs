using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class ForumEmotion
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int PostId { get; set; }
    [ForeignKey("PostId")]
    public ForumPost ForumPost { get; set; }
    
    [Required]
    public int AccountId { get; set; }
    [ForeignKey("AccountId")]
    public Account Account { get; set; }
    
    
}