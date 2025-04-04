using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class UserExamFavorite
{
    [Key]
    public int Id { get; set; }
    
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    public int ExamId { get; set; }
    [ForeignKey("ExamId")]
    public Exam Exam { get; set; }
}