
using System.ComponentModel.DataAnnotations;

namespace backend.Controllers.Flashcard.Request;

public class FlashcardSetRequest
{
    [Required]
    [MaxLength(50)]
    public string SetName { get; set; }
    public string Description { get; set; }
    public bool IsPublic { get; set; } = false;
    [Required]
    public int FlashcardFolderId { get; set; }
    [Required]
    public int UserId { get; set; }
    
}