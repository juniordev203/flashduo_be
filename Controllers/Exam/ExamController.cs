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
    [ProducesResponseType(typeof(List<ExamResponse>), 200)]
    public async Task<ActionResult<List<ExamResponse>>> GetExams()
    {
        var exams = await _context.Exam
            .Select(e => new ExamResponse
            {
                Id = e.Id,
                ExamName = e.ExamName,
                Description = e.Description,
                TotalQuestions = e.TotalQuestions,
            })
            .ToListAsync();
        return Ok(exams ?? new List<ExamResponse>());
    }

    [HttpGet("exam-detail/{examId}")]
    [ProducesResponseType(typeof(ExamResponse), 200)]
    public async Task<ActionResult<List<ExamResponse>>> GetExamsById(int examId)
    {
        var examById = await _context.Exam
            .Where(e => e.Id == examId)
            .Select(e => new ExamResponse
            {
                Id = e.Id,
                ExamName = e.ExamName,
                Description = e.Description,
                TotalQuestions = e.TotalQuestions,
            })
            .FirstOrDefaultAsync();
        if (examById == null)
            return NotFound("Không tìm thấy bài thi");

        return Ok(examById);

    }
    //api lay cau hoi cho de thi dc chon
    [HttpGet("exam-start/{examId}")]
    [ProducesResponseType(typeof(ExamResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetExamQuestion(int examId)
    {
        var examStartById = await _context.Exam
            .Where(e => e.Id == examId)
            .Select(e => new ExamResponse
            {
                Id = e.Id,
                ExamName = e.ExamName,
                Description = e.Description,
                TotalQuestions = e.TotalQuestions,
                ExamQuestion = _context.ExamQuestion
                    .Where(eq => eq.ExamId == e.Id && eq.Question != null)
                    .OrderBy(eq => eq.QuestionOrder)
                    .Select(eq => new ExamQuestionResponse
                    {
                        Id = eq.Id,
                        ExamId = eq.ExamId,
                        QuestionId = eq.QuestionId,
                        Question = new QuestionResponse
                        {
                            Id = eq.Question.Id,
                            Part = eq.Question.Part,
                            Content = eq.Question.Content,
                            ImageUrl = eq.Question.ImageUrl,
                            AudioUrl = eq.Question.AudioUrl,
                            Explanation = eq.Question.Explanation,
                            IsMultipleChoice = eq.Question.IsMultipleChoice,
                            QuestionAnswer = eq.Question.QuestionAnswer
                                .Select(qa => new QuestionAnswerResponse
                                {
                                    Id = qa.Id,
                                    OptionLabel = qa.OptionLabel,
                                    OptionText = qa.OptionText,
                                    IsAnswer = qa.IsAnswer,
                                }).ToList()
                        },
                    }).ToList()
            }).FirstOrDefaultAsync();
        if (examStartById == null)
        {
            return NotFound("Khong tim thay bai thi");
        }
        return Ok(examStartById);
    }

    //luu dap ap vao db
    [HttpPost("user-answer")]
    public async Task<IActionResult> PushUserAnswer([FromBody] UserAnswer userAnswer)
    {
        return Ok();
    }
}

