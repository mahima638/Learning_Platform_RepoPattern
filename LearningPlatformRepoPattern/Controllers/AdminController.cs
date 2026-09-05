using Microsoft.AspNetCore.Mvc;

namespace LearningPlatformRepoPattern.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {

            var role = HttpContext.Session.GetString("Role");

            if (role != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }
    }
}
