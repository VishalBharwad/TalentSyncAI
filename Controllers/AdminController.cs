using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalentSyncAI.Data;
using TalentSyncAI.Enums;

namespace TalentSyncAI.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            ViewBag.TotalRecruiters = await _context.Companies.CountAsync();

            ViewBag.PendingRecruiters = await _context.Companies
                .CountAsync(c => c.Status == CompanyStatus.Pending);

            ViewBag.ApprovedRecruiters = await _context.Companies
                .CountAsync(c => c.Status == CompanyStatus.Approved);

            // We haven't created these tables yet
            ViewBag.TotalCandidates = 0;
            ViewBag.TotalJobs = 0;

            // 👇 Add these lines here
            ViewBag.RecentRecruiters = await _context.Companies
                .OrderByDescending(c => c.CreatedAt)
                .Take(5)
                .ToListAsync();

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