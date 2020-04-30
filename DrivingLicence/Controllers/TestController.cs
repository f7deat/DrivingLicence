using System;
using System.Linq;
using System.Threading.Tasks;
using DrivingLicence.Data;
using DrivingLicence.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DrivingLicence.Controllers
{
    [Authorize]
    public class TestController : Controller
    {
        private readonly ApplicationDbContext _context;
        public TestController(ApplicationDbContext context) => _context = context;
        public async Task<IActionResult> Index(int? examId)
        {
            if (examId == null)
            {
                return NotFound("Can't not find the exam");
            }
            var quizs = await _context.Quizzes.Where(x => x.ExamId == examId).ToListAsync();
            if (quizs.Count() == 0)
            {
                return NotFound("Không có câu hỏi trong đề thi này!");
            }
            var test = new Test
            {
                // Lấy thông tin đề hiện tại
                Exam = await _context.Exams.FindAsync(examId),
                // Lấy thông tin câu hỏi
                Quizzes = quizs
            };
            return View(test);
        }

        [HttpPost]
        public async Task<IActionResult> End(Test test)
        {
            // lấy danh sách câu hỏi của đề thi hiện tại
            var quizs = await _context.Quizzes.Where(x => x.ExamId == test.ExamId).ToListAsync();
            // các field lưu chung
            var consequence = new Consequence
            {
                SubmitedDate = DateTime.Now,
                UserId = User.Identity.Name,
                CorrectCount = 0,
                ExamId = test.ExamId
            };
            foreach (var item in test.Answers)
            {
                // kiểm tra đáp án có đúng không
                if (_context.Quizzes.Find(item.QuizId).Answers == item.Choices)
                {
                    consequence.CorrectCount += 1;
                }
            }
            // số câu hỏi sai
            consequence.WrongCount = quizs.Count() - consequence.CorrectCount;
            // tính điểm: tổng điểm = 100
            consequence.Score = (100 / quizs.Count()) * consequence.CorrectCount;

            await _context.AddAsync(consequence);

            await _context.SaveChangesAsync();

            return RedirectToAction("details", "consequences", new { id = consequence.ConsequenceId });
        }
    }
}