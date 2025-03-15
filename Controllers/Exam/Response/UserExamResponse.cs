using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers.Exam.Response
{
public class UserExamResponse
    {
        public int UserExamId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ExamData ExamData { get; set; }
    }

    public class ExamData
    {
        public int ExamId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public List<QuestionData> Questions { get; set; }
    }

    public class QuestionData
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public List<OptionData> Options { get; set; }
    }

    public class OptionData
    {
        public int Id { get; set; }
        public string Content { get; set; }
    }
}