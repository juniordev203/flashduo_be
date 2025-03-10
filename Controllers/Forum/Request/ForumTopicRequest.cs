using System.ComponentModel.DataAnnotations;

namespace backend.Controllers.Forum.Response;

public class ForumTopicRequest
{
    [Required]
    public string Title { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
}