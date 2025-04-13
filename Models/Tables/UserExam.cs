using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class UserExam
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public int UserId { get; set; }
    [Required]
    public int ExamId { get; set; }
    public DateTime StartTime { get; set; } = DateTime.UtcNow;
    public DateTime EndTime { get; set; } = DateTime.UtcNow;
    public int DurationSeconds { get; set; }
    public ExamStatus Status { get; set; } = ExamStatus.NotStarted;
    public int ScoreReading { get; set; }  
    public int ScoreListening { get; set; }
    public int TotalScore { get; set; }
    public bool IsSubmitted { get; internal set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    [ForeignKey("ExamId")]
    public Exam Exam { get; set; }
    
    public List<UserAnswer> UserAnswers { get; set; } = new();

}
public enum ExamStatus
{
    NotStarted = 0,
    InProgress = 1,
    Completed = 2
}

