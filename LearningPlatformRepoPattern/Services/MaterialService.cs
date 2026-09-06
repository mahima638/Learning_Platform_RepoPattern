using LearningPlatformRepoPattern.Data;
using LearningPlatformRepoPattern.Models;
using LearningPlatformRepoPattern.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LearningPlatformRepoPattern.Services
{
    public class MaterialService : IMaterialRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public MaterialService(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }


        public List<Material> GetAll()
        {
            return _context.Materials
                .Include(m => m.MasterCourse)
                .Include(m => m.SubCourse)
                .Include(m => m.Topic)
                .Include(m => m.Mcqs)
                .OrderByDescending(m => m.Id)
                .ToList();
        }


        public Material GetById(int id)
        {
            return _context.Materials
                .Include(m => m.MasterCourse)
                .Include(m => m.SubCourse)
                .Include(m => m.Topic)
                .Include(m => m.Mcqs)
                .FirstOrDefault(m => m.Id == id);
        }


        public List<MasterCourse> GetMasterCourses()
        {
            return _context.MasterCourses
                .OrderBy(c => c.CourseName)
                .ToList();
        }


        public List<SubCourse> GetSubCourses(int masterCourseId)
        {
            return _context.SubCourses
                .Where(s => s.MasterCourseId == masterCourseId)
                .OrderBy(s => s.SubCourseName)
                .ToList();
        }


        public List<Topic> GetTopics(int subCourseId)
        {
            return _context.Topics
                .Where(t => t.SubCourseId == subCourseId)
                .OrderBy(t => t.TopicName)
                .ToList();
        }


        public string Add(Material material, IFormFile assignmentFile, List<Mcq> mcqs)
        {
            if (material.MasterCourseId <= 0)
            {
                return "Please select Master Course.";
            }

            if (material.SubCourseId <= 0)
            {
                return "Please select Sub Course.";
            }

            if (material.TopicId <= 0)
            {
                return "Please select Topic.";
            }

            if (assignmentFile == null)
            {
                return "Please select an Assignment file.";
            }

            if (mcqs == null || mcqs.Count != 3)
            {
                return "Please add exactly 3 MCQ questions before saving the material.";
            }

            foreach (var mcq in mcqs)
            {
                if (string.IsNullOrWhiteSpace(mcq.Question) ||
                    string.IsNullOrWhiteSpace(mcq.Option1) ||
                    string.IsNullOrWhiteSpace(mcq.Option2) ||
                    string.IsNullOrWhiteSpace(mcq.Option3) ||
                    string.IsNullOrWhiteSpace(mcq.Option4) ||
                    string.IsNullOrWhiteSpace(mcq.Answer))
                {
                    return "Please complete all MCQ details.";
                }
            }


            // Save assignment file
            material.Assignment = SaveAssignment(assignmentFile);


            // Save material
            _context.Materials.Add(material);
            _context.SaveChanges();


            // Save MCQs
            foreach (var mcq in mcqs)
            {
                mcq.MaterialId = material.Id;

                _context.Mcqs.Add(mcq);
            }

            _context.SaveChanges();

            return "Material added successfully!";
        }



        public string Update(Material material, IFormFile assignmentFile, List<Mcq> mcqs)
        {
            var existing = _context.Materials.Find(material.Id);


            if (existing == null)
            {
                return "Material not found.";
            }


            if (material.MasterCourseId <= 0)
            {
                return "Please select Master Course.";
            }

            if (material.SubCourseId <= 0)
            {
                return "Please select Sub Course.";
            }

            if (material.TopicId <= 0)
            {
                return "Please select Topic.";
            }


            if (mcqs == null || mcqs.Count != 3)
            {
                return "Please add exactly 3 MCQ questions before saving the material.";
            }


            foreach (var mcq in mcqs)
            {
                if (string.IsNullOrWhiteSpace(mcq.Question) ||
                    string.IsNullOrWhiteSpace(mcq.Option1) ||
                    string.IsNullOrWhiteSpace(mcq.Option2) ||
                    string.IsNullOrWhiteSpace(mcq.Option3) ||
                    string.IsNullOrWhiteSpace(mcq.Option4) ||
                    string.IsNullOrWhiteSpace(mcq.Answer))
                {
                    return "Please complete all MCQ details.";
                }
            }


            // Update material information
            existing.MasterCourseId = material.MasterCourseId;

            existing.SubCourseId = material.SubCourseId;

            existing.TopicId = material.TopicId;


            // Update assignment only
            // if a new file is selected
            if (assignmentFile != null)
            {
                existing.Assignment = SaveAssignment(assignmentFile);
            }


            // Remove old MCQs

            var existingMcqs = _context.Mcqs.Where(m => m.MaterialId == material.Id).ToList();


            if (existingMcqs.Count > 0)
            {
                _context.Mcqs.RemoveRange(existingMcqs);
            }


            // Add new MCQs

            foreach (var mcq in mcqs)
            {
                mcq.Id = 0;

                mcq.MaterialId = material.Id;

                _context.Mcqs.Add(mcq);
            }

            _context.SaveChanges();

            return "Material updated successfully!";
        }




        public string Delete(int id)
        {
            var material = _context.Materials.Find(id);


            if (material == null)
            {
                return "Material not found.";
            }


            // Delete MCQs first
            var mcqs = _context.Mcqs.Where(m => m.MaterialId == id).ToList();


            if (mcqs.Count > 0)
            {
                _context.Mcqs.RemoveRange(mcqs);
            }


            // Delete material
            _context.Materials.Remove(material);

            _context.SaveChanges();

            return "Material deleted successfully!";
        }



        private string SaveAssignment(IFormFile assignmentFile)
        {
            string fileName = Guid.NewGuid() + Path.GetExtension(assignmentFile.FileName);


            string folderPath =Path.Combine(_env.WebRootPath, "uploads");


            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }


            string fullPath =  Path.Combine(folderPath, fileName);


            using (var stream =  new FileStream(fullPath, FileMode.Create))
            {
                assignmentFile.CopyTo(stream);
            }

            return "/uploads/" + fileName;
        }
    }
}