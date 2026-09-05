using LearningPlatformRepoPattern.Models;
using LearningPlatformRepoPattern.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LearningPlatformRepoPattern.Controllers
{
    public class UserController : Controller
    {
        IUserRepository up;
        public UserController(IUserRepository up)
        {
            this.up = up;

        }
        public IActionResult Dashboard()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = up.GetUserById(userId.Value);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(user);
        }
        [HttpGet]
        public IActionResult EditProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = up.GetUserById(userId.Value);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new EditProfile
            {
                UserId = user.UserId,
                UserName = user.UserName,
                UserEmail = user.UserEmail
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult EditProfile(EditProfile model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = up.GetUserById(model.UserId);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            user.UserName = model.UserName;
            user.UserEmail = model.UserEmail;

            up.UpdateUser(user);

            return RedirectToAction("Profile");
        }
    }
}
