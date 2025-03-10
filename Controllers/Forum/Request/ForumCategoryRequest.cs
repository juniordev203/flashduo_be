using System.ComponentModel.DataAnnotations;

namespace backend.Controllers.Forum.Response;

public class ForumCategoryRequest
{
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
}