using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Controllers.Exam.Response
{
    public class ExamResponse
    {
        public int Id { get; set; }
        public string ExamName { get; set; }
        public string Description { get; set; }
        public int TotalQuestions { get; set; }
        public List<ExamQuestionResponse> ExamQuestion { get; set; } = new ();
    }
    public class ExamQuestionResponse
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public int QuestionId { get; set; }
        public QuestionResponse Question { get; set; } = new ();
    }
    public class QuestionResponse
    {
        public int Id { get; set; }
        public QuestionSection Section { get; set; }
        public QuestionPart Part { get; set; }
        public string Content { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public string AudioUrl { get; set; } = "";
        public string Explanation { get; set; } = "";
        public bool IsMultipleChoice { get; set; }
        public List<QuestionAnswerResponse> QuestionAnswers { get; set; } = new ();
        
    }
    public class QuestionAnswerResponse {
        public int Id { get; set;}
        public string OptionLabel { get; set; }
        public string OptionText { get; set; } = "";
        public bool IsAnswer { get; set; }
    }
}