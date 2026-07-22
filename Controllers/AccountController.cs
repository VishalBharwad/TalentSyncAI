using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TalentSyncAI.Models.Identity;
using TalentSyncAI.Models.ViewModels;

namespace TalentSyncAI.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid Email or Password.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName!,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction("Dashboard", "Admin");
                }

                if (await _userManager.IsInRoleAsync(user, "Recruiter"))
                {
                    return RedirectToAction("Dashboard", "Recruiter");
                }

                if (await _userManager.IsInRoleAsync(user, "Candidate"))
                {
                    return RedirectToAction("Dashboard", "Candidate");
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid Email or Password.");
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }
    }
}