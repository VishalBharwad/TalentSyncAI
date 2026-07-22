using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TalentSyncAI.Data;
using TalentSyncAI.Models.Identity;
using TalentSyncAI.Models.Entities;
using TalentSyncAI.Models.ViewModels;

namespace TalentSyncAI.Controllers
{
    [Authorize(Roles = "Candidate")]
    public class CandidateController : Controller
    {
        
        public IActionResult Dashboard()
        {
            return View();
        }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public CandidateController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> CompleteProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var candidate = await _context.Candidates
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (candidate == null)
                return NotFound();

            var model = new CandidateProfileViewModel
            {
                DateOfBirth = candidate.DateOfBirth,
                Gender = candidate.Gender,
                Address = candidate.Address,
                City = candidate.City,
                State = candidate.State,
                Country = candidate.Country,
                Pincode = candidate.Pincode
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteProfile(CandidateProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var candidate = await _context.Candidates
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (candidate == null)
                return NotFound();

            candidate.DateOfBirth = model.DateOfBirth;
            candidate.Gender = model.Gender;
            candidate.Address = model.Address;
            candidate.City = model.City;
            candidate.State = model.State;
            candidate.Country = model.Country;
            candidate.Pincode = model.Pincode;

            candidate.ProfileCompletionPercentage = 40;
            candidate.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Profile updated successfully.";

            return RedirectToAction(nameof(Dashboard));
        }
    }
}