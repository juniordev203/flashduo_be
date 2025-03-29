using backend.Models;

namespace backend.Controllers.Exam.Request;

public class UserAnswerRequest
{
    public int UserId { get; set; }
    public List<AnswerChoiceRequest> answerChoice { get; set; } = new();
}

public class AnswerChoiceRequest
{
    public int QuestionId { get; set; }
    public QuestionSection Section { get; set; }
    public string OptionLabel  { get; set; } = string.Empty;

}