using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalentSyncAI.Data;

namespace TalentSyncAI.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> Recruiters()
        {
            var recruiters = await _context.Companies
                .Include(c => c.Recruiter)
                .ToListAsync();

            return View(recruiters);
        }
    }
}