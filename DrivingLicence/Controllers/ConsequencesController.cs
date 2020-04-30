using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DrivingLicence.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace DrivingLicence.Controllers
{
    [Authorize]
    public class ConsequencesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConsequencesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles ="admin")]
        public async Task<IActionResult> Index(string id)
        {
            // Lấy quyền hiện tại
            var roles = ((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);

            if (roles.Any(x => x == "admin"))
            {
                // hiển thị tất cả bài làm nếu là admin
                if (string.IsNullOrEmpty(id))
                {
                    var applicationDbContext = _context.Consequences.Include(c => c.Exam);
                    return View(await applicationDbContext.ToListAsync());
                }
                else
                {
                    // hiển thị theo một user nào đó
                    var applicationDbContext = _context.Consequences.Include(c => c.Exam).Where(x=>x.UserId == id);
                    return View(await applicationDbContext.ToListAsync());
                }
            }
            else
            {
                // chỉ hiển thị bài làm của người dùng hiện tại
                var applicationDbContext = _context.Consequences.Include(c => c.Exam).Where(x=>x.UserId == User.Identity.Name);
                return View(await applicationDbContext.ToListAsync());
            }
        }

        // GET: Consequences/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consequence = await _context.Consequences
                .Include(c => c.Exam)
                .FirstOrDefaultAsync(m => m.ConsequenceId == id);
            if (consequence == null)
            {
                return NotFound();
            }

            return View(consequence);
        }

        // GET: Consequences/Create
        public IActionResult Create()
        {
            ViewData["ExamId"] = new SelectList(_context.Exams, "ExamId", "ExamId");
            return View();
        }

        // POST: Consequences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConsequenceId,SubmitedDate,Score,ExamId,CorrectCount,WrongCount,UserId")] Consequence consequence)
        {
            if (ModelState.IsValid)
            {
                _context.Add(consequence);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExamId"] = new SelectList(_context.Exams, "ExamId", "ExamId", consequence.ExamId);
            return View(consequence);
        }

        // GET: Consequences/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consequence = await _context.Consequences.FindAsync(id);
            if (consequence == null)
            {
                return NotFound();
            }
            ViewData["ExamId"] = new SelectList(_context.Exams, "ExamId", "ExamId", consequence.ExamId);
            return View(consequence);
        }

        // POST: Consequences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConsequenceId,SubmitedDate,Score,ExamId,CorrectCount,WrongCount,UserId")] Consequence consequence)
        {
            if (id != consequence.ConsequenceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consequence);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsequenceExists(consequence.ConsequenceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExamId"] = new SelectList(_context.Exams, "ExamId", "ExamId", consequence.ExamId);
            return View(consequence);
        }

        // GET: Consequences/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consequence = await _context.Consequences
                .Include(c => c.Exam)
                .FirstOrDefaultAsync(m => m.ConsequenceId == id);
            if (consequence == null)
            {
                return NotFound();
            }

            return View(consequence);
        }

        // POST: Consequences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consequence = await _context.Consequences.FindAsync(id);
            _context.Consequences.Remove(consequence);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsequenceExists(int id)
        {
            return _context.Consequences.Any(e => e.ConsequenceId == id);
        }
    }
}
