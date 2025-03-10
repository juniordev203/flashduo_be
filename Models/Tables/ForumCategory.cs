using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class ForumCategory
{
    [Key]
    public int Id { get; set; }

    [Required] 
    public string Name { get; set; }
    public string Description { get; set; }
    
    public List<ForumTopic> Topics { get; set; }
}