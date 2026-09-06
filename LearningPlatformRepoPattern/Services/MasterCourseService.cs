using LearningPlatformRepoPattern.Data;
using LearningPlatformRepoPattern.Models;
using LearningPlatformRepoPattern.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LearningPlatformRepoPattern.Services
{
    public class MasterCourseService : IMasterCourseRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public MasterCourseService(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public List<MasterCourse> GetAll()
        {
            return _context.MasterCourses.OrderByDescending(c => c.CreatedAt).ToList();
        }

        public MasterCourse GetById(int id)
        {
            return _context.MasterCourses.Find(id);
        }

        public List<MasterCourse> GetActiveCourses()
        {
            return _context.MasterCourses.Where(m => m.Status == "Active").ToList();
        }

        public string Add(MasterCourse course, IFormFile thumbnail, string createdBy)
        {
            if (thumbnail != null)
            {
                course.ThumbnailPath = SaveThumbnail(thumbnail);
            }

            course.CreatedAt = DateTime.Now;
            course.CreatedBy = string.IsNullOrEmpty(createdBy) ? "" : createdBy;

            _context.MasterCourses.Add(course);
            _context.SaveChanges();

            return "Master Course added successfully!";
        }

        public string Update(MasterCourse course, IFormFile thumbnail)
        {
            var existing = _context.MasterCourses.Find(course.Id);
            if (existing == null)
            {
                return "Master Course not found.";
            }

            existing.CourseName = course.CourseName;
            existing.Status = course.Status;

            if (thumbnail != null)
            {
                existing.ThumbnailPath = SaveThumbnail(thumbnail);
            }

            _context.SaveChanges();

            return "Master Course updated successfully!";
        }

        public string Delete(int id)
        {
            var course = _context.MasterCourses.Find(id);
            if (course == null)
            {
                return "Master Course not found.";
            }

            _context.MasterCourses.Remove(course);
            _context.SaveChanges();

            return "Master Course deleted successfully!";
        }

        private string SaveThumbnail(IFormFile thumbnail)
        {
            string fileName = Guid.NewGuid() + Path.GetExtension(thumbnail.FileName);
            string folderPath = Path.Combine(_env.WebRootPath, "uploads");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fullPath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                thumbnail.CopyTo(stream);
            }

            return "/uploads/" + fileName;
        }
    }
}