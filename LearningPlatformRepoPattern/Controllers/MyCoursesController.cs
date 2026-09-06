using LearningPlatformRepoPattern.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearningPlatformRepoPattern.Controllers
{
    public class MyCoursesController : Controller
    {
        private readonly IMyCoursesService _service;

        public MyCoursesController(IMyCoursesService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(int userId)
        {
            var courses = await _service.GetMyCourses(userId);

            return View(courses);
        }
    }
}