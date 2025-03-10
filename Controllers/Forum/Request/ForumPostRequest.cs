using System.ComponentModel.DataAnnotations;

namespace backend.Controllers.Forum.Response;

public class ForumPostRequest
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Content { get; set; }
    public int TopicId { get; set; }
    public int AccountId { get; set; }
}