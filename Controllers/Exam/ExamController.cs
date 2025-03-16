using backend.Controllers.Exam.Response;
using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace backend.Controllers.Exam;

[ApiExplorerSettings(GroupName = "Exam")]
[Tags("Exam")]
[ApiController]
[Route("api/Exam")]
public class ExamController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public ExamController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    // API lấy toàn bộ đề thi
    [HttpGet("exam")]
    public async Task<IActionResult> GetExams()
    {
        var exams = await _context.Exam.ToListAsync();
        return Ok(exams);
    }
    //api lay cau hoi cho de thi dc chon
    [HttpGet("exam-question")]
    public async Task<IActionResult> GetExamQuestion(int examId)
    {
        var examQuestions = await _context.ExamQuestion
            .Where(eq => eq.ExamId == examId)
            .Include(eq => eq.Question)
            .ThenInclude(q => q.QuestionAnswer)
            .OrderBy(eq => eq.QuestionOrder)
            .Select(eq => new QuestionResponse
            {
                Id = eq.QuestionId,
                Part = eq.Question.Part,
                Content = eq.Question.Content,
                ImageUrl = eq.Question.ImageUrl,
                AudioUrl = eq.Question.AudioUrl,
                Explanation = eq.Question.Explanation,
                IsMultipleChoice = eq.Question.IsMultipleChoice,
                QuestionAnswer = eq.Question.QuestionAnswer
                    .Select(
                        qa => new QuestionAnswerResponse {
                            Id = qa.Id,
                            OptionLabel = qa.OptionLabel,
                            OptionText = qa.OptionText,
                            IsAnswer = qa.IsAnswer,
                        }
                    ).ToList(),
            }).ToListAsync();
        return Ok(examQuestions);
    }
    //luu dap ap vao db
    [HttpPost("user-answer")]
    public async Task<IActionResult> PushUserAnswer([FromBody] UserAnswer userAnswer)
    {
        return Ok();
    }
}

