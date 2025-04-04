using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class UserFlashcardSet
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }
    public int SetId { get; set; }

    public int TotalCards { get; set; }
    public int KnownCards { get; set; }

    public bool IsCompleted { get; set; } = false;
    public DateTime LastReviewedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("UserId")]
    public User User { get; set; }

    [ForeignKey("SetId")]
    public FlashcardSet Set { get; set; }
}
