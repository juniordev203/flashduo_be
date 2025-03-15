using System.ComponentModel.DataAnnotations;
using backend.Models;

namespace backend.Controllers.Exam.Response
{
    public class UserExamRequest
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ExamId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? DurationSeconds { get; set; }
        public ExamStatus Status { get; set; }
        public int? ScoreReading { get; set; }
        public int? ScoreListening { get; set; }
        public int? TotalScore { get; set; }
        
    }
    public class CreateUserExamRequest
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ExamId { get; set; }
    }
    public class UpdateUserExamScoreRequest {
        public int? ScoreReading { get; set; }
        public int? ScoreListening { get; set; }
        public int? TotalScore { get; set; }
    }
    public class UpdateUserExamStatusRequest 
{
    [Required]
    public ExamStatus Status { get; set; }
    
    public DateTime? EndTime { get; set; }
}
}