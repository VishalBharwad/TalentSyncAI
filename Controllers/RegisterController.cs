using Microsoft.AspNetCore.Mvc;
using TalentSyncAI.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using TalentSyncAI.Data;
using TalentSyncAI.Models.Entities;
using TalentSyncAI.Models.Identity;
using TalentSyncAI.Enums;

namespace TalentSyncAI.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public RegisterController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: /Register/Recruiter
        [HttpGet]
        public IActionResult Recruiter()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Recruiter(RecruiterRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser
            {
                FullName = model.FullName,
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

            await _userManager.AddToRoleAsync(user, "Recruiter");
            var company = new Company
            {
                CompanyName = model.CompanyName,
                RecruiterId = user.Id,
                Status = CompanyStatus.Pending,
                ProfileCompleted = false,
                CreatedAt = DateTime.Now
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            // Next Step: Assign Recruiter Role

            TempData["Success"] = "Registration submitted successfully. Please wait for admin approval.";

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Candidate()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Candidate(CandidateRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser
            {
                FullName = model.FullName,
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

            await _userManager.AddToRoleAsync(user, "Candidate");

            var candidate = new Candidate
            {
                UserId = user.Id,
                ProfileCompleted = false,
                CreatedAt = DateTime.Now
            };

            _context.Candidates.Add(candidate);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Registration completed successfully. Please login.";

            return RedirectToAction("Login", "Account");
        }
    }
}