using backend.Controllers.Auth.request;
using backend.Controllers.Auth.Response;
using backend.Controllers.Forum.Response;
using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;


namespace backend.Controllers.Forum;

[ApiExplorerSettings(GroupName = "Forum")]
[Tags("Forum")]
[ApiController]
[Route("api/[controller]")]
public class ForumController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public ForumController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    // API BAI VIET
    // lay tat ca bai viet
    [HttpGet("posts")]
    public async Task<IActionResult> GetAllPosts()
    {
        var posts = await _context.ForumPosts
            .Include(p => p.Account)
            .ThenInclude(a => a.User)
            .Select(p => new ForumPostResponse
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                CreatedAt = p.CreateAt,
                AccountId = p.Account.Id,
                AuthorName = p.Account.User.FullName,
                AvataUrl = p.Account.User.AvatarUrl,
                
            }).ToArrayAsync();
        return Ok(posts);
    }
    // lay bai viet theo id
    [HttpGet("posts/{id}")]
    public async Task<IActionResult> GetPostById(int id)
    {
        var posts = await _context.ForumPosts
            .Include(p => p.Account)
            .ThenInclude(a => a.User)
            .Where(p => p.Id == id)
            .Select(p => new ForumPostResponse
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                CreatedAt = p.CreateAt,
                AccountId = p.Account.Id,
                AuthorName = p.Account.User.FullName,
                AvataUrl = p.Account.User.AvatarUrl,
            }).FirstOrDefaultAsync();
        if (posts == null)
        {
            return NotFound("Bài viết không tồn tại!");
        }
        return Ok(posts);
    }
    //tao bai viet moi
    [HttpPost("posts")]
    public async Task<IActionResult> AddPost([FromBody] ForumPostRequest request)
    {
        var post = new ForumPost
        {
            Title = request.Title,
            Content = request.Content,
            TopicId = request.TopicId,
            AccountId = request.AccountId,
        };
        _context.ForumPosts.Add(post);
        // luu bai viet vao db
        await _context.SaveChangesAsync(); 
        return CreatedAtAction(nameof(GetPostById), new { postId = post.Id }, post);
    }
    // xoa bai viet
    [HttpDelete("posts/{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var post = await _context.ForumPosts.FindAsync(id);
        if (post == null) return NotFound("Bài viết không tồn tại!");
        
        _context.ForumPosts.Remove(post);
        await _context.SaveChangesAsync();
        return Ok("Bài viết đã bị xoá!");
    }
    // api topic
    // api lay cac topic
    [HttpGet("topic")]
    public async Task<IActionResult> GetALlPostTopics()
    {
        var topics = await _context.ForumTopics
            .Include(t => t.ForumCategory)
            .Select(t => new ForumTopicResponse
            {
                Id = t.Id,
                Title = t.Title,
                CategoryId = t.CategoryId,
                CategoryName = t.ForumCategory.Name,
            }).ToArrayAsync();
            return Ok(new { message = topics });
    }
// tao topic
    [HttpPost("topic")]
    public async Task<IActionResult> AddPostTopic([FromBody] ForumTopic request)
    {
        _context.ForumTopics.Add(request);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPostById), new { postId = request.Id }, request);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetALlCategories()
    {
        var categories = await _context.ForumCategories.ToArrayAsync();
        return Ok(new { message = categories });
    }

    [HttpPost("categories")]
    public async Task<IActionResult> AddPostCategory([FromBody] ForumCategory request)
    {
        _context.ForumCategories.Add(request);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetALlCategories), new { id = request.Id }, request);
    }
    
}