using backend.Controllers.Exam.Response;
using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using backend.Controllers.Exam.Request;
using Microsoft.AspNetCore.Authorization;

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

    // GET
    [HttpGet("exam")]
    [ProducesResponseType(typeof(List<ExamResponse>), 200)]
    public async Task<ActionResult<List<ExamResponse>>> GetExams()
    {
        var exams = await _context.Exams
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
        var examById = await _context.Exams
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
    
    [HttpGet("exam-start/{examId}")]
    [ProducesResponseType(typeof(ExamResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetExamQuestion(int examId)
    {
        var examStartById = await _context.Exams
            .Where(e => e.Id == examId)
            .Select(e => new ExamResponse
            {
                Id = e.Id,
                ExamName = e.ExamName,
                Description = e.Description,
                TotalQuestions = e.TotalQuestions,
                ExamQuestion = _context.ExamQuestions
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
                            Section = eq.Question.Section,
                            Part = eq.Question.Part,
                            Content = eq.Question.Content,
                            ImageUrl = eq.Question.ImageUrl,
                            AudioUrl = eq.Question.AudioUrl,
                            Explanation = eq.Question.Explanation,
                            IsMultipleChoice = eq.Question.IsMultipleChoice,
                            QuestionAnswers = eq.Question.QuestionAnswers
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

    [HttpGet("user-exam/{userExamId}/score")]
    [ProducesResponseType(typeof(UserExamScoreResponse), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetExamScore(int userExamId)
    {
        var userExam = await _context.UserExams
            .Include(ue => ue.UserAnswers)
            .FirstOrDefaultAsync(ue => ue.Id == userExamId);

        if (userExam == null)
        {
            return BadRequest("Khong tim thay bai thi");
        }

        var userAnswers = userExam.UserAnswers;
        
        var questionIdsListening = userAnswers
            .Where(ua => ua.Section == QuestionSection.Listening)
            .Select(eq => eq.QuestionId)
            .ToList();
            
        var questionIdsReading = userAnswers
            .Where(ua => ua.Section == QuestionSection.Reading)
            .Select(eq => eq.QuestionId)
            .ToList();

        var correctAnswers = await _context.QuestionAnswers
            .Where(qa => qa.IsAnswer == true && qa.QuestionId.HasValue && 
                         (questionIdsListening.Contains(qa.QuestionId.Value) || 
                          questionIdsReading.Contains(qa.QuestionId.Value)))
            .ToDictionaryAsync(qa => qa.QuestionId.Value, qa => qa.OptionLabel);

        var userAnswersDict = userAnswers
            .ToDictionary(ua => ua.QuestionId, ua => ua.UserAnswerChoice);

        int totalUserAnswerCorrectReading = 0;
        int totalUserAnswerCorrectListening = 0;
        int scoreListening = 0;
        int scoreReading = 0;

        foreach (int questionId in questionIdsListening)
        {
            if (userAnswersDict.TryGetValue(questionId, out var userAnswer) && 
                correctAnswers.TryGetValue(questionId, out var correctAnswer) && 
                userAnswer == correctAnswer)
            {
                totalUserAnswerCorrectListening++;
            }
        }

        if (totalUserAnswerCorrectListening == 0)
        {
            scoreListening = 5;
        }

        if (totalUserAnswerCorrectListening > 0 && totalUserAnswerCorrectListening < 96)
        {
            scoreListening = totalUserAnswerCorrectListening * 5 + 10;
        }
        else if (totalUserAnswerCorrectListening >= 96)
        {
            scoreListening = 495;
        }

        foreach (int questionId in questionIdsReading)
        {
            if (userAnswersDict.TryGetValue(questionId, out var userAnswer) && 
                correctAnswers.TryGetValue(questionId, out var correctAnswer) && 
                userAnswer == correctAnswer)
            {
                totalUserAnswerCorrectReading++;
            }
        }

        if (totalUserAnswerCorrectReading >= 0 && totalUserAnswerCorrectReading <= 2)
        {
            scoreReading = 5;
        }
        else if (totalUserAnswerCorrectReading > 2 && totalUserAnswerCorrectReading <= 100)
        {
            scoreReading = (totalUserAnswerCorrectReading - 1) * 5;
        }

        userExam.ScoreListening = scoreListening;
        userExam.ScoreReading = scoreReading;
        userExam.TotalScore = scoreListening + scoreReading;
        userExam.Status = ExamStatus.Completed;
        await _context.SaveChangesAsync();

        return Ok(new UserExamScoreResponse
        {
            UserId = userExam.UserId,
            UserExamId = userExam.Id,
            ScoreListening = userExam.ScoreListening,
            ScoreReading = userExam.ScoreReading,
        });
    }

    [HttpGet("user-exam/{userId}/favorite")]
    [ProducesResponseType(typeof(UserExamFavoriteResponse), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetExamFavoriteByUserId(int userId)
    {
        var favoriteExams = await _context.UserExamFavorites
            .Where(uf => uf.UserId == userId)
            .Select(uf => uf.Exam)
            .ToListAsync();

        if (favoriteExams == null || !favoriteExams.Any())
        {
            return NotFound("No favorite exams found for this user.");
        }
        return Ok(favoriteExams);
    }

    [HttpGet("user-exam/{userId}/result-exams")]
    [ProducesResponseType(typeof(UserExamResultResponse), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetExamResultByUserId(int userId)
    {
        var examResultByUserId = await _context.UserExams
            .Include(u => u.Exam)
            .Where(uf => uf.UserId == userId && uf.Status == ExamStatus.Completed)
            .Select(uf => new UserExamResultResponse
            {
                Id = uf.Id,
                ExamName = uf.Exam.ExamName,
                ScoreListening = uf.ScoreListening,
                ScoreReading = uf.ScoreReading,
                TotalScore = uf.TotalScore,
                StartTime = uf.StartTime,
                EndTime = uf.EndTime,
            }).ToListAsync();
        
        return Ok(examResultByUserId);
    }
    
    
    
    
    //POST
    [HttpPost("user-exam/start")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> PostUserExam([FromBody] UserExamBaseRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        var userExam = new UserExam
        {
            ExamId = request.ExamId,
            UserId = request.UserId,
            Status = request.Status,
            StartTime = DateTime.UtcNow,
        };
        await _context.UserExams.AddAsync(userExam);
        await _context.SaveChangesAsync();
        return Ok(userExam.Id);
    }

    [HttpPost("user-answer/{userExamId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> PostUserAnswer(int userExamId, [FromBody] UserAnswerRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest("Yêu cầu không thể là null");
            }

            if (request.answerChoice == null || request.answerChoice.Count == 0)
            {
                return BadRequest("Danh sách câu trả lời không thể trống");
            }

            var userExam = await _context.UserExams
                .FirstOrDefaultAsync(ua => ua.Id == userExamId);

            if (userExam == null)
            {
                return BadRequest("Không tìm thấy bài thi với ID: " + userExamId);
            }

            // Xóa câu trả lời cũ nếu có
            var existingAnswers = await _context.UserAnswers
                .Where(ua => ua.UserExamId == userExamId)
                .ToListAsync();

            if (existingAnswers.Any())
            {
                _context.UserAnswers.RemoveRange(existingAnswers);
            }

            // Thêm câu trả lời mới
            var userAnswers = request.answerChoice.Select(a => new UserAnswer
            {
                UserExamId = userExam.Id,
                QuestionId = a.QuestionId, // Đảm bảo tên thuộc tính khớp với client
                Section = a.Section, // Đảm bảo tên thuộc tính khớp với client
                UserAnswerChoice = a.OptionLabel // Đảm bảo tên thuộc tính khớp với client
            }).ToList();

            await _context.UserAnswers.AddRangeAsync(userAnswers);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Lưu đáp án thành công!" });
        }
        catch (Exception ex)
        {
            return BadRequest($"Lỗi khi lưu đáp án: {ex.Message}");
        }
    }

    [HttpPost("user-exam/favorite")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> AddUserExamFavorites(int userId, int examId)
    {
        var existingFavorite = await _context.UserExamFavorites
            .FirstOrDefaultAsync((uf => uf.UserId==userId && uf.ExamId==examId));
        if (existingFavorite != null)
        {
            return BadRequest("Bai thi da duoc yeu thich");
        }

        var userExamFavorite = new UserExamFavorite
        {
            ExamId = examId,
            UserId = userId,
        };
        await _context.UserExamFavorites.AddAsync(userExamFavorite);
        await _context.SaveChangesAsync();
        return Ok();
    }
}

