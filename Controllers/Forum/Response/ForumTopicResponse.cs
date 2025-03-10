namespace backend.Controllers.Forum.Response;

public class ForumTopicResponse
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
}