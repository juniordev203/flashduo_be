using backend.Models;

namespace backend.Controllers.Forum.Response;

public class ForumPostResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public int TopicId { get; set; }
    public string TopicTitle { get; set; }
    
    public int AccountId { get; set; }
    public string AuthorName { get; set; }
    public string AvataUrl { get; set; }
}