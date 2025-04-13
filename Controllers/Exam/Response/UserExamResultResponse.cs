namespace backend.Controllers.Exam.Response;

public class UserExamResultResponse
{
    public int Id { get; set; }
    public string ExamName { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int ScoreReading { get; set; }
    public int ScoreListening { get; set; } 
    public int TotalScore { get; set; }
}