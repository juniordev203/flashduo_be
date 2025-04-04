using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class FlashcardTest
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string TestType { get; set; } 
    [Required]
    public int Score { get; set; }

    [Required] public int TotalQuestions { get; set; } = 8;
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    
    public int FlashcardSetId { get; set; }
    [ForeignKey("FlashcardSetId")]
    public FlashcardSet FlashcardSet { get; set; }
    
    public DateTime StartTime { get; set; } = DateTime.UtcNow;
    public DateTime EndTime { get; set; }

    public List<Flashcard> Flashcards { get; set; } = new();

}