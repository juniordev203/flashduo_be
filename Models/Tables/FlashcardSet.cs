using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class FlashcardSet
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public string SetName { get; set; }
    public string Description { get; set; }
    public bool IsPublic { get; set; } = false;
    public int FlashcardFolderId { get; set; }
    [ForeignKey("FlashcardFolderId")]
    public FlashcardFolder FlashcardFolder { get; set; }
    
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public List<Flashcard> Flashcards { get; set; } = new();
    [Required] public int TotalCards { get; set; } = 0;

}