using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class Exam
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string ExamName { get; set; }
    public string Description { get; set; }
    [Required]
    public int TotalQuestions { get; set; }
    
    public List<ExamQuestion> ExamQuestion { get; set; }
}