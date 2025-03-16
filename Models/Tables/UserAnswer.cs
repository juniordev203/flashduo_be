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
    public int ExamQuestionId { get; set; }
    [Required]
    public char UserAnswerChoice { get; set; }  // A, B, C, D

    [ForeignKey("UserExamId")]
    public UserExam UserExam { get; set; }

    [ForeignKey("ExamQuestionId")]
    public ExamQuestion ExamQuestion { get; set; }
}