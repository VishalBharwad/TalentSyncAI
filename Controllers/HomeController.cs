using Microsoft.AspNetCore.Mvc;

namespace TalentSyncAI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Jobs()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}