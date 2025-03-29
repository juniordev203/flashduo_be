using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class ExamQuestion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int ExamId { get; set; }

    [ForeignKey("ExamId")]
    public Exam Exam { get; set; }

    [Required]
    public int QuestionId { get; set; }

    [ForeignKey("QuestionId")]
    public Question Question { get; set; }

    [Required]
    public int QuestionOrder { get; set; }  // Thứ tự câu hỏi trong đề thi
    
    public List<UserAnswer> UserAnswers { get; set; }
}
