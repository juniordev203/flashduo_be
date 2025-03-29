using backend.Models;

namespace backend.Controllers.Exam.Request;

public class UserExamBaseRequest
{
    public int UserId { get; set; }
    public int ExamId { get; set; }
    public ExamStatus Status { get; set; } = ExamStatus.InProgress;
}