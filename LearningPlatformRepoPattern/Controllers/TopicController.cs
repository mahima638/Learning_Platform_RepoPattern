using LearningPlatformRepoPattern.Models;
using LearningPlatformRepoPattern.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LearningPlatformRepoPattern.Controllers
{
    public class TopicController : Controller
    {
        private readonly ITopicRepository _service;

        public TopicController(ITopicRepository service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            ViewBag.MasterCourses = _service.GetMasterCourses();

            var topics = _service.GetAll();

            return View(topics);
        }

        [HttpGet]
        public IActionResult GetSubCourses(int masterCourseId)
        {
            var subCourses = _service.GetSubCourses(masterCourseId);

            var result = subCourses.Select(s => new
                {
                    id = s.Id,
                    name = s.SubCourseName
                });

            return Json(result);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var topic = _service.GetById(id);

            if (topic == null)
            {
                return Json(null);
            }

            return Json(new
            {
                id = topic.Id,
                masterCourseId = topic.MasterCourseId,
                subCourseId = topic.SubCourseId,
                topicName = topic.TopicName,
                videoUrl = topic.VideoUrl,
                status = topic.Status,
                thumbnailPath = topic.ThumbnailPath
            });
        }

        [HttpPost]
        public IActionResult Add( Topic model, IFormFile thumbnail)
        {
            string createdBy = HttpContext.Session.GetString("DisplayName") ?? "System";

            string message = _service.Add(model, thumbnail, createdBy);

            TempData["Message"] = message;

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit( Topic model, IFormFile thumbnail)
        {
            string message = _service.Update( model, thumbnail);

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