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
[Route("api/exam")]
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
    [HttpGet]
    public async Task<IActionResult> GetExams()
    {
        var exams = await _context.Exam.ToListAsync();
        return Ok(exams);
    }
}

