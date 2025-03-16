using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers.Exam.Response
{
    public class ExamResponse
    {
        public int Id { get; set; }
        public string ExamName { get; set; }
        public string Description { get; set; }
        public int TotalQuestions { get; set; }
    }
}