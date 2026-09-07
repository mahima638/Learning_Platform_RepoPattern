using LearningPlatformRepoPattern.Models;
using LearningPlatformRepoPattern.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LearningPlatformRepoPattern.Controllers
{
    public class MyCoursesController : Controller
    {
        private readonly IMyCoursesRepository _service;

        public MyCoursesController(
            IMyCoursesRepository service)
        {
            _service = service;
        }

        //My courses
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var courses = await _service.GetMyCourses(userId.Value);

            return View(courses);
        }

        // watch video

        [HttpGet]
        public async Task<IActionResult> WatchVideo(int sid,int userId,int? tid)
        {
            var model = await _service.GetWatchVideo(sid,userId,tid);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }


        //download assignment
        [HttpGet]
        public async Task<IActionResult> DownloadAssignment(int sid)
        {
            var result =await _service.DownloadAssignment(sid);

            if (result.FileBytes == null)
            {
                TempData["Message"] ="Assignment is not available.";

                return RedirectToAction(nameof(WatchVideo),
                    new
                    {
                        sid = sid
                    });
            }

            return File(result.FileBytes,"application/octet-stream",result.FileName);
        }


        //submit assignment

        [HttpPost]
        public async Task<IActionResult> SubmitAssignment(
            int sid,
            IFormFile assignmentFile)
        {
            var result =await _service.SubmitAssignment(assignmentFile);

            if (result)
            {
                TempData["Message"] ="Assignment submitted successfully.";
            }
            else
            {
                TempData["Message"] ="Please select a valid file.";
            }

            return RedirectToAction(
                nameof(WatchVideo),
                new
                {
                    sid = sid,
                    userId = 1
                });
        }


        //submit mcq
        [HttpPost]
        public async Task<IActionResult> SubmitMcq(int sid,int userId,int tid,
            List<string> answers)
        {
            var result =await _service.SubmitMcq(sid,userId,tid,answers);

            TempData["McqMessage"] =result.Message;
            TempData["McqScore"] =result.Score;
            TempData["McqTotal"] =result.Total;
            TempData["McqPassed"] =result.Passed;

            return RedirectToAction(nameof(WatchVideo),
                new
                {
                    sid = sid,
                    userId = userId,
                    tid = tid
                });
        }


        //certificate
        [HttpGet]
        public async Task<IActionResult> Certificate(int sid,int userId)
        {
            var model =await _service.GetCertificate(sid,userId);
            if (model == null)
            {
                return NotFound(
                    "Certificate cannot be generated. " +
                    "Please make sure all topics are completed.");
            }

            return PartialView("_Certificate",model);
        }
    }
}