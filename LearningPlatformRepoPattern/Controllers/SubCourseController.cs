using LearningPlatformRepoPattern.Models;
using LearningPlatformRepoPattern.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LearningPlatformRepoPattern.Controllers
{
    public class SubCourseController : Controller
    {
        private readonly ISubCourseRepository _service;
        private readonly IMasterCourseRepository _masterCourseService;

        public SubCourseController(ISubCourseRepository service, IMasterCourseRepository masterCourseService)
        {
            _service = service;
            _masterCourseService = masterCourseService;
        }
        public IActionResult Index()
        {
            var subCourses = _service.GetAll();

            ViewBag.MasterCourseList = new SelectList(
                _masterCourseService.GetActiveCourses(),
                "Id",
                "CourseName"
            );
            return View(subCourses);
        }

        [HttpPost]
        public IActionResult Add(SubCourse model)
        {
            string username = HttpContext.Session.GetString("DisplayName") ?? "System";
            string message = _service.Add(model, username);
            TempData["Message"] = message;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(SubCourse model)
        {
            string message = _service.Update(model);
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
