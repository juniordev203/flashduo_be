using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Controllers.Exam.Response
{
    public class QuestionResponse
    {
        public int Id { get; set; }
        public QuestionPart Part { get; set; }
        public string Content { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public string AudioUrl { get; set; } = "";
        public string Explanation { get; set; } = "";
        public bool IsMultipleChoice { get; set; }
        public List<QuestionAnswerResponse> QuestionAnswer { get; set; } = new ();
        
    }
    public class QuestionAnswerResponse {
        public int Id { get; set;}
        public char OptionLabel { get; set; }
        public string OptionText { get; set; } = "";
        public bool IsAnswer { get; set; }
    }
}