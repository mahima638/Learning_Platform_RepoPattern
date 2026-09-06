using LearningPlatformRepoPattern.Models;
using LearningPlatformRepoPattern.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningPlatformRepoPattern.Controllers
{
    public class MasterCourseController : Controller
    {
        private readonly IMasterCourseRepository _service;

        public MasterCourseController(IMasterCourseRepository service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var courses = _service.GetAll();
            return View(courses);
        }

        [HttpPost]
        public IActionResult Add(MasterCourse model, IFormFile thumbnail)
        {
            string username = HttpContext.Session.GetString("DisplayName") ?? "System";
            string message = _service.Add(model, thumbnail, username);
            TempData["Message"] = message;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(MasterCourse model, IFormFile thumbnail)
        {
            string message = _service.Update(model, thumbnail);
            TempData["Message"] = message;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            string message = _service.Delete(id);
            TempData["Message"] = message;
            return RedirectToAction("Index");
        }
    }
}