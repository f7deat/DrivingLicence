using DrivingLicence.Data;
using System.Collections.Generic;

namespace DrivingLicence.Models
{
    public class Test
    {
        public int ExamId { get; set; }
        public List<YourChoice> Answers { get; set; }
        public Exam Exam { get; set; }
        public List<Quiz> Quizzes { get; set; }
    }
    public class YourChoice
    {
        public int QuizId { get; set; }
        public string Choices { get; set; }
        public bool IsCorrect { get; set; }
    }
}
