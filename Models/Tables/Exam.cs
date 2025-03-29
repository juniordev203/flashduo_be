using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Exam
{
    [Key]
    public int Id { get; set; }

    [Required] public string ExamName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [Required] public int TotalQuestions { get; set; } = 0;
    [Required] public List<ExamQuestion> ExamQuestions { get; set; } = new();
}