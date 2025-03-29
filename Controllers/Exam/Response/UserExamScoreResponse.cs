namespace backend.Controllers.Exam.Response;

public class UserExamScoreResponse
{
    public int UserId { get; set; }
    public int UserExamId { get; set; }
    public int ScoreListening { get; set; }
    public int ScoreReading { get; set; }
}