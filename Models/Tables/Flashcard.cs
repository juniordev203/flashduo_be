using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class Flashcard
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] public string TermLanguage { get; set; } = "en";
    [Required] public string DefinitionLanguage { get; set; } = "vi";
    
    public string ImageUrl { get; set; }
    public string AudioUrl { get; set; }
    
    public bool IsFavorite { get; set; } = false;
    public bool IsDeleted { get; set; } = false;

    public int CorrectCount { get; set; } = 0;
    public int WrongCount { get; set; } = 0;
    public DateTime? LastReviewedAt { get; set; }
    
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    
    public int FlashcardSetId { get; set; }
    [ForeignKey("FlashcardSetId")]
    public FlashcardSet FlashcardSet { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
}