using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentSyncAI.Models.ViewModels;

namespace TalentSyncAI.Controllers
{
    [Authorize(Roles = "Recruiter")]
    public class RecruiterController : Controller
    {


        public IActionResult Dashboard()
        {
            var model = new RecruiterDashboardViewModel
            {
                RecruiterName = User.Identity?.Name ?? "Recruiter",

                TotalJobs = 0,

                TotalApplications = 0,

                TotalInterviews = 0,

                TotalShortlisted = 0
            };

            return View(model);
        }

        public IActionResult CompanyProfile()
        {
            return View();
        }

        public IActionResult PostJob()
        {
            return View();
        }

        public IActionResult ManageJobs()
        {
            return View();
        }

        public IActionResult Applications()
        {
            return View();
        }

        public IActionResult Interviews()
        {
            return View();
        }
    }
}