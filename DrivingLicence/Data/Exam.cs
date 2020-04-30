using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DrivingLicence.Data
{
    public class Exam
    {
        public int ExamId { get; set; }
        [Display(Name ="Tên kỳ thi")]
        public string Name { get; set; }
        [Display(Name ="Mô tả")]
        public string Description { get; set; }
        [Display(Name ="Ngày tạo")]
        public DateTime CreatedDate { get; set; }
        [Display(Name ="Ngày cập nhật")]
        public DateTime ModifiedDate { get; set; }
        [Display(Name="Thời gian (phút)")]
        public int Time { get; set; }
        public bool Status { get; set; }
        [Display(Name = "Người tạo")]
        public string UserId { get; set; }

        public virtual ICollection<Quiz> Quizzes { get; set; }
        public virtual ICollection<Consequence> Consequences { get; set; }
    }
}
