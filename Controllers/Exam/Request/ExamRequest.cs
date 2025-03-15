namespace backend.Controllers.Exam.Request;

public class ExamRequest
{
    public string ExamName { get; set; }
    public string Description { get; set; }
    public int TotalQuestions { get; set; }
}