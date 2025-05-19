using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class UserAnswer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int UserExamId { get; set; }

    [Required]
    public int QuestionId { get; set; }
    [Required]
    public QuestionSection Section { get; set; }

    [Required]
    public string UserAnswerChoice { get; set; }

    // Quan hệ với UserExam
    [ForeignKey("UserExamId")]
    public virtual UserExam UserExam { get; set; }
    [ForeignKey("QuestionId")]
    public virtual Question Question { get; set; }
}
