using System;
using System.ComponentModel.DataAnnotations;

namespace DrivingLicence.Data
{
    public class Consequence
    {
        public int ConsequenceId { get; set; }
        [Display(Name = "Ngày thi")]
        public DateTime SubmitedDate { get; set; }
        [Display(Name ="Điểm")]
        public int Score { get; set; }
        [Display(Name = "Đề thi")]
        public int ExamId { get; set; }
        [Display(Name ="Số câu đúng")]
        public int CorrectCount { get; set; }
        [Display(Name ="Số câu sai")]
        public int WrongCount { get; set; }
        [Display(Name ="Thí sinh")]
        public string UserId { get; set; }

        public virtual Exam Exam { get; set; }
    }
}
