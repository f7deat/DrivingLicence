using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DrivingLicence.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace DrivingLicence.Controllers
{
    [Authorize]
    public class QuizsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public QuizsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Quizs
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Quizzes.Include(q => q.Exam);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Quizs/Details/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quizzes
                .Include(q => q.Exam)
                .FirstOrDefaultAsync(m => m.QuizId == id);
            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create(int examId)
        {
            // lấy ra đề thi hiện tại
            ViewBag.ExamId = examId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("QuizId,ExamId,Question,ChoiceA,ChoiceB,ChoiceC,ChoiceD,Answers,Media,CreatedDate,ModifiedDate")] Quiz quiz, IFormFile media)
        {
            if (ModelState.IsValid)
            {
                if (media != null)
                {
                    // xử lý hình ảnh được tải lên
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, "media");
                    var filePath = Path.Combine(path, media.FileName);
                    using var stream = System.IO.File.Create(filePath);
                    await media.CopyToAsync(stream);
                    quiz.Media = media.FileName;
                }
                // lưu thông tin câu hỏi
                quiz.CreatedDate = DateTime.Now;
                quiz.ModifiedDate = DateTime.Now;
                _context.Add(quiz);
                await _context.SaveChangesAsync();
                // sau khi lưu xong sẽ chuyển đến trang chi tiết của đề này
                return RedirectToAction("details", "exams", new { id = quiz.ExamId });
            }
            return View(quiz);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }
            return View(quiz);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("QuizId,ExamId,Question,ChoiceA,ChoiceB,ChoiceC,ChoiceD,Answers,Media,CreatedDate,ModifiedDate")] Quiz quiz, IFormFile media)
        {
            if (id != quiz.QuizId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (media != null)
                    {
                        string path = Path.Combine(_webHostEnvironment.WebRootPath, "media");
                        var filePath = Path.Combine(path, media.FileName);
                        using var stream = System.IO.File.Create(filePath);
                        await media.CopyToAsync(stream);
                        quiz.Media = media.FileName;
                    }
                    _context.Update(quiz);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuizExists(quiz.QuizId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("details", "exams", new { id = quiz.ExamId });
            }
            return View(quiz);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quizzes
                .Include(q => q.Exam)
                .FirstOrDefaultAsync(m => m.QuizId == id);
            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();
            return RedirectToAction("details","exams", new { id = quiz.ExamId });
        }

        private bool QuizExists(int id)
        {
            return _context.Quizzes.Any(e => e.QuizId == id);
        }
    }
}
