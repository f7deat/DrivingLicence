using System;
using System.ComponentModel.DataAnnotations;

namespace DrivingLicence.Data
{
    public class Quiz
    {
        [Key]
        public int QuizId { get; set; }
        public int ExamId { get; set; }
        [Display(Name = "Câu hỏi")]
        public string Question { get; set; }
        [Display(Name = "Đáp án A")]
        public string ChoiceA { get; set; }
        [Display(Name = "Đáp án B")]
        public string ChoiceB { get; set; }
        [Display(Name = "Đáp án C")]
        public string ChoiceC { get; set; }
        [Display(Name = "Đáp án D")]
        public string ChoiceD { get; set; }
        [Display(Name ="Đáp án đúng")]
        public string Answers { get; set; }
        [Display(Name ="Hình ảnh")]
        public string Media { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual Exam Exam { get; set; }
    }
}
