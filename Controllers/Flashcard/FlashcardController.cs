using backend.Controllers.Flashcard.Request;
using backend.Controllers.Flashcard.Response;
using backend.Data;
using backend.Models;
using backend.Models.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers.Flashcard;
[ApiExplorerSettings(GroupName = "Flashcard")]
[Tags("Flashcard")]
[ApiController]
[Authorize]
[Route("api/Flashcard")]
public class FlashcardController : ControllerBase
{
    public readonly AppDbContext _context;
    public readonly IConfiguration _configuration;

    public FlashcardController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    
    //GET
    [HttpGet("flashcard-folders/{userId}")]
    [ProducesResponseType(typeof(FlashcardFolderResponse), 200)]
    public async Task<IActionResult> GetFlashcardFolders(int userId)
    {
        var folders = await _context.FlashcardFolders
            .Where(f => f.UserId == userId)
            .Select(f => new FlashcardFolderResponse
            {
                Id = f.Id,
                FolderName = f.FolderName,
                UserId = f.UserId,
            }).ToListAsync();
        return Ok(folders);
    }
    [HttpGet("flashcard-sets/folder/{folderId}")]
    [ProducesResponseType(typeof(FlashcardSetResponse), 200)]
    public async Task<IActionResult> GetFlashcardSetsByFolderId(int folderId)
    {
        var setsByFolderId = await _context.FlashcardSets
            .Where(s => s.FlashcardFolderId == folderId)
            .Select(f => new FlashcardSetResponse
            {
                Id = f.Id,
                SetName = f.SetName,
                Description = f.Description,
                IsPublic = f.IsPublic,
                FlashcardFolderId = f.FlashcardFolderId,
                UserId = f.UserId,
                CreatedAt = f.CreatedAt,
                TotalCards = f.TotalCards,
            }).ToListAsync();
        return Ok(setsByFolderId);
    }
    [HttpGet("flashcard-sets/user/{userId}")]
    [ProducesResponseType(typeof(FlashcardSetResponse), 200)]
    public async Task<IActionResult> GetFlashcardSetsByUserId(int userId)
    {
        var sets = await _context.FlashcardSets
            .Where(f => f.UserId == userId)
            .Select(f => new FlashcardSetResponse
            {
                Id = f.Id,
                SetName = f.SetName,
                Description = f.Description,
                IsPublic = f.IsPublic,
                FlashcardFolderId = f.FlashcardFolderId,
                UserId = f.UserId,
                CreatedAt = f.CreatedAt,
                TotalCards = f.TotalCards,
            }).ToListAsync();
        return Ok(sets);
    }
    [HttpGet("flashcard-set/details/{setId}")]
    [ProducesResponseType(typeof(FlashcardSetDetailResponse), 200)]
    public async Task<IActionResult> GetFlashcardSetWithFlashcards(int id)
    {
        var set = await _context.FlashcardSets
            .Include(s => s.Flashcards)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (set == null)
            return NotFound();

        var response = new FlashcardSetDetailResponse
        {
            Id = set.Id,
            SetName = set.SetName,
            Description = set.Description,
            IsPublic = set.IsPublic,
            FlashcardFolderId = set.FlashcardFolderId,
            UserId = set.UserId,
            CreatedAt = set.CreatedAt,
            TotalCards = set.TotalCards,
            Flashcards = set.Flashcards.Select(f => new FlashcardResponse
            {
                Id = f.Id,
                TermLanguage = f.TermLanguage,
                DefinitionLanguage = f.DefinitionLanguage,
                ImageUrl = f.ImageUrl,
                AudioUrl = f.AudioUrl,
                FlashcardSetId = f.FlashcardSetId
            }).ToList()
        };

        return Ok(response);
    }
    [HttpGet("flashcard/set/{setId}")]
    [ProducesResponseType(typeof(FlashcardFavoritesResponse), 200)]
    public async Task<IActionResult> GetFlashcardBySetId(int setId)
    {
        var flashcardsBySetId = await _context.Flashcards
            .Where(f => f.FlashcardSetId == setId)
            .Select(f => new FlashcardFavoritesResponse
            {
                Id = f.Id,
                TermLanguage = f.TermLanguage,
                DefinitionLanguage = f.DefinitionLanguage,
                ImageUrl = f.ImageUrl,
                AudioUrl = f.AudioUrl,
                FlashcardSetId = f.FlashcardSetId,
                IsFavorite = f.IsFavorite
            }).ToListAsync();
        return Ok(flashcardsBySetId);
    }   
    [HttpGet("flashcard/favorites/{userId}")]
    [ProducesResponseType(typeof(FlashcardFavoritesResponse), 200)]
    public async Task<IActionResult> GetFavoriteFlashcards(int userId)
    {
        var flashcards = await _context.Flashcards
            .Include(f => f.FlashcardSet)
            .Where(f => f.IsFavorite && f.FlashcardSet.UserId == userId)
            .Select(f => new FlashcardFavoritesResponse
            {
                Id = f.Id,
                TermLanguage = f.TermLanguage,
                DefinitionLanguage = f.DefinitionLanguage,
                ImageUrl = f.ImageUrl,
                AudioUrl = f.AudioUrl,
                FlashcardSetId = f.FlashcardSetId,
                IsFavorite = f.IsFavorite,
            })
            .ToListAsync();

        return Ok(flashcards);
    }
    
    // game
    [HttpGet("flashcard/game/result/{userId}")]
    [ProducesResponseType(typeof(FlashcardGameResultByUserResponse), 200)]
    public async Task<IActionResult> GetGameResultByUserId(int userId)
    {
        var gameResultUser = await _context.UserFlashcardGames
            .Where(g => g.UserId == userId)
            .Select(ufg => new FlashcardGameResultByUserResponse
            {
                Id = ufg.Id,
                CreatedAt = ufg.CreatedAt,
                SetName = ufg.FlashcardSet.SetName,
            }).ToListAsync();
        return Ok(gameResultUser);
    }
    [HttpGet("flashcard/game/result/{setId}")]
    [ProducesResponseType(typeof(FlashcardGameResultByUserResponse), 200)]
    public async Task<IActionResult> GetGameResultBySetId(int setId)
    {
        var gameResultUser = await _context.UserFlashcardGames
            .Where(g => g.SetId == setId)
            .Select(ufg => new FlashcardGameResultBySetResponse
            {
                Id = ufg.Id,
                CreatedAt = ufg.CreatedAt,
                UserName = ufg.User.FullName,
            }).ToListAsync();
        return Ok(gameResultUser);
    }
    
    //POST
    [HttpPost("flashcard-folder")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> PostFlashcardFolder([FromBody] FlashcardFolderRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        var flashcardFolder = new FlashcardFolder
        {
            FolderName = request.FolderName,
            UserId = request.UserId,
        };
        await _context.FlashcardFolders.AddAsync(flashcardFolder);
        await _context.SaveChangesAsync();
        return Ok();
    }
    [HttpPost("flashcard-set")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> PostFlashcardSet([FromBody] FlashcardSetRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        var falshcardSet = new FlashcardSet
        {
            SetName = request.SetName,
            Description = request.Description,
            IsPublic = request.IsPublic,
            FlashcardFolderId = request.FlashcardFolderId,
            UserId = request.UserId,
        };
        await _context.FlashcardSets.AddAsync(falshcardSet);
        await _context.SaveChangesAsync();
        return Ok();
    }
    [HttpPost("flashcard")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> PostFlashcard([FromBody] FlashcardRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        var flashcard = new Models.Flashcard
        {
            TermLanguage = request.TermLanguage,
            DefinitionLanguage = request.DefinitionLanguage,
            ImageUrl = request.ImageUrl,
            AudioUrl = request.AudioUrl,
            FlashcardSetId = request.FlashcardSetId,
            UserId = request.UserId,
        };
        await _context.Flashcards.AddAsync(flashcard);
        var set = await _context.FlashcardSets.FindAsync(request.FlashcardSetId);
        if (set != null)
        {
            set.TotalCards += 1;
        }
        await _context.SaveChangesAsync();
        return Ok();
    }
    
    //game 
    private double NormalizeTimeToSeconds(int timeInMilliseconds)
    {
        // Giả sử timeInMilliseconds = 6600, kết quả trả về: 6.6
        return Math.Round(timeInMilliseconds / 10.0, 1);
    }

    [HttpPost("flashcard/game/result")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> PostFlashcardGameResult([FromBody] FlashcardGameResultRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }
        var normalizedTime = NormalizeTimeToSeconds(request.DurationTime);
        var flashcardGameResult = new UserFlashcardGame
        {
            UserId = request.UserId,
            SetId = request.SetId,
            DurationTime = normalizedTime,
            CreatedAt = DateTime.UtcNow,
            TotalWord = request.TotalWord,
        };
        await _context.UserFlashcardGames.AddAsync(flashcardGameResult);
        await _context.SaveChangesAsync();
        return Ok();
    }
    //PUT
    [HttpPut("flashcard/{id}/favorite")]
    public async Task<IActionResult> ToggleFlashcardFavorite(int id)
    {
        var flashcard = await _context.Flashcards.FindAsync(id);
        if (flashcard == null)
        {
            return NotFound();
        }
        flashcard.IsFavorite = !flashcard.IsFavorite;
        await _context.SaveChangesAsync();
        return Ok();
    }
    [HttpPut("flashcard/folder-name/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> ToggleUpdateFolderName(int id, string name)
    {
        var folder = await _context.FlashcardFolders.FindAsync(id);
        if (folder == null) { return BadRequest(); }
        folder.FolderName = name;
        folder.UpdatedNameAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return Ok();
    }
    [HttpPut("flashcard/set-name/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> ToggleUpdateSetName(int id, string name)
    {
        var set = await _context.FlashcardSets.FindAsync(id);
        if (set == null) { return BadRequest(); }
        set.SetName = name;
        await _context.SaveChangesAsync();
        return Ok();
    }
    
    //DELETE
    [HttpDelete("flashcard-folder-delete/{id}")]
    public async Task<IActionResult> DeleteFlashcardFolder(int id)
    {
        var folder = await _context.FlashcardFolders.FindAsync(id);
        if (folder == null)
            return NotFound();

        _context.FlashcardFolders.Remove(folder);
        await _context.SaveChangesAsync();

        return Ok();
    }
    [HttpDelete("flashcard-set-delete/{id}")]
    public async Task<IActionResult> DeleteFlashcardSet(int id)
    {
        var set = await _context.FlashcardSets.FindAsync(id);
        if (set == null)
        {
            return NotFound();
        }
        _context.FlashcardSets.Remove(set);
        await _context.SaveChangesAsync();
        return Ok();
    }
    [HttpDelete("flashcard-delete/{id}")]
    public async Task<IActionResult> DeleteFlashcard(int id)
    {
        var flashcard = await _context.Flashcards.FindAsync(id);
        if (flashcard == null)
        {
            return NotFound();
        }
        var set = await _context.FlashcardSets.FindAsync(flashcard.FlashcardSetId);
        if (set != null && set.TotalCards > 0)
        {
            set.TotalCards -= 1;
        }
        _context.Flashcards.Remove(flashcard);
        await _context.SaveChangesAsync();
        return Ok();
    }
    

}
