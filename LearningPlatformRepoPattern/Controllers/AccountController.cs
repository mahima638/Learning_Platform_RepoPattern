using LearningPlatformRepoPattern.Models;
using LearningPlatformRepoPattern.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningPlatformRepoPattern.Controllers
{
    public class AccountController : Microsoft.AspNetCore.Mvc.Controller
    {
        IUserRepository up;
        public AccountController(IUserRepository up)
        {
            this.up = up;
            
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public IActionResult Login(Login model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.UserEmail == "admin@gmail.com" && model.Password == "Admin@123")
            {
                HttpContext.Session.SetString("Role", "Admin");
                HttpContext.Session.SetString("DisplayName", "Admin");

                return RedirectToAction("Dashboard", "Admin");
            }

            var user = up.GetUserByEmailAndPassword(
                 model.UserEmail,
                 model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            HttpContext.Session.SetInt32("UserId", user.UserId);

            return RedirectToAction("Dashboard", "User");
        }
        [HttpPost]
        public IActionResult Register(Registeration model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User
            {
                UserName = model.UserName,
                UserEmail = model.UserEmail,
                UserPassword = model.Password
            };

           
            up.AddUser(user);
            return RedirectToAction("Login");
        }


    }
}
