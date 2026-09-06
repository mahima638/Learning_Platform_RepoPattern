using LearningPlatformRepoPattern.Models;
using LearningPlatformRepoPattern.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace LearningPlatformRepoPattern.Controllers
{
    public class MaterialController : Controller
    {
        private readonly IMaterialRepository _service;

        public MaterialController(
            IMaterialRepository service)
        {
            _service = service;
        }



        public IActionResult Index()
        {
            ViewBag.MasterCourseList = new SelectList( _service.GetMasterCourses(),"Id", "CourseName");

            var materials = _service.GetAll();

            return View(materials);
        }


        
        [HttpGet]
        public IActionResult GetSubCourses(int masterCourseId)
        {
            var subCourses =_service.GetSubCourses(masterCourseId);

            var result =
                subCourses.Select(s => new
                {
                    id = s.Id,
                    name = s.SubCourseName
                });

            return Json(result);
        }



        [HttpGet]
        public IActionResult GetTopics(int subCourseId)
        {
            var topics =_service.GetTopics(subCourseId);

            var result =
                topics.Select(t => new
                {
                    id = t.Id,
                    name = t.TopicName
                });

            return Json(result);
        }



        [HttpGet]
        public IActionResult GetById(int id)
        {
            var material =_service.GetById(id);

            if (material == null)
            {
                return Json(null);
            }


            var mcqs = material.Mcqs.Select(m => new
                {
                    id = m.Id,
                    question = m.Question,
                    option1 = m.Option1,
                    option2 = m.Option2,
                    option3 = m.Option3,
                    option4 = m.Option4,
                    answer = m.Answer
                }).ToList();


            return Json(new
            {
                id = material.Id,

                masterCourseId = material.MasterCourseId,

                subCourseId =material.SubCourseId,

                topicId = material.TopicId,

                assignment = material.Assignment,

                mcqs = mcqs
            });
        }



        [HttpPost]
        public IActionResult Add( Material model, IFormFile assignmentFile, List<Mcq> mcqs)
        {
          
            model.Mcqs = null;


            string message =_service.Add(model, assignmentFile, mcqs);


            TempData["Message"] = message;


            return RedirectToAction("Index");
        }


        
        [HttpPost]
        public IActionResult Edit(Material model, IFormFile assignmentFile, List<Mcq> mcqs)
        {
            
            model.Mcqs = null;


            string message =_service.Update(
                    model,
                    assignmentFile,
                    mcqs);


            TempData["Message"] = message;


            return RedirectToAction("Index");
        }


       

        [HttpPost]
        public IActionResult Delete(int id)
        {
            string message =_service.Delete(id);


            TempData["Message"] = message;


            return RedirectToAction("Index");
        }
    }
}