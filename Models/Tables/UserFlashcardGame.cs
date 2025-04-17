using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models.Tables
{
    public class UserFlashcardGame
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SetId { get; set; }
        public int TotalWord { get; set; }
        public double DurationTime { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("SetId")]
        public FlashcardSet FlashcardSet { get; set; }
    }
}