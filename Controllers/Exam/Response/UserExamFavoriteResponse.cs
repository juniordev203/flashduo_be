namespace backend.Controllers.Exam.Response;

public class UserExamFavoriteResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ExamId { get; set; }
    public ExamBaseResponse Exam { get; set; }
}

public class ExamBaseResponse
{
    public int Id { get; set; }
    public string ExamName { get; set; }
    public string Description { get; set; }
    public int TotalQuestions { get; set; }
}