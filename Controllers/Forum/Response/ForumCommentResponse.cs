namespace backend.Controllers.Forum.Response;

public class ForumCommentResponse
{
    public int Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public int PostId { get; set; }
    
    public int AccountId { get; set; }
    public string AuthorName { get; set; }
    public string AvatarUrl { get; set; }
}