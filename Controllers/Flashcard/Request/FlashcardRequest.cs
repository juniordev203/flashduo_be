using System.ComponentModel.DataAnnotations;

namespace backend.Controllers.Flashcard.Request;

public class FlashcardRequest
{
    [Required]
    public string TermLanguage { get; set; }
    [Required]
    public string DefinitionLanguage { get; set; }
    public string ImageUrl { get; set; }
    public string AudioUrl { get; set; }
    
    [Required]
    public int FlashcardSetId { get; set; }
}