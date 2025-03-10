using System.ComponentModel.DataAnnotations;

namespace backend.Controllers.Forum.Response;

public class ForumCommentRequest
{
    [Required]
    public string Content { get; set; }
    public int PostId { get; set; }
    public int AccountId { get; set; }
}