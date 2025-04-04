using System.ComponentModel.DataAnnotations;

namespace backend.Controllers.Flashcard.Request;

public class FlashcardFolderRequest
{
    [Required]
    [MaxLength(50)]  
    public string FolderName { get; set; }
    [Required]
    public int UserId { get; set; }
    
}