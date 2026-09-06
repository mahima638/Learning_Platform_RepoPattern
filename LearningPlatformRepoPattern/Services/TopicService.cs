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
    public class TopicService : ITopicRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TopicService(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public List<Topic> GetAll()
        {
            return _context.Topics
                .Include(t => t.MasterCourse)
                .Include(t => t.SubCourse)
                .OrderByDescending(t => t.Id)
                .ToList();
        }

        public Topic GetById(int id)
        {
            return _context.Topics
                .Include(t => t.MasterCourse)
                .Include(t => t.SubCourse)
                .FirstOrDefault(t => t.Id == id);
        }

        public List<MasterCourse> GetMasterCourses()
        {
            return _context.MasterCourses.ToList();
        }

        public List<SubCourse> GetSubCourses(int masterCourseId)
        {
            return _context.SubCourses.Where(s => s.MasterCourseId == masterCourseId).ToList();
        }

        public string Add(Topic topic, IFormFile thumbnail, string createdBy)
        {
            bool exists = _context.Topics.Any(t => t.MasterCourseId == topic.MasterCourseId && 
            t.SubCourseId == topic.SubCourseId && t.TopicName == topic.TopicName);

            if (exists)
            {
                return "Topic already exists.";
            }

            if (thumbnail != null)
            {
                topic.ThumbnailPath = SaveThumbnail(thumbnail);
            }

            _context.Topics.Add(topic);
            _context.SaveChanges();

            return "Topic added successfully!";
        }

        public string Update(Topic topic, IFormFile thumbnail)
        {
            var existing = _context.Topics.Find(topic.Id);

            if (existing == null)
            {
                return "Topic not found.";
            }

            bool exists = _context.Topics.Any(t => t.Id != topic.Id &&
                t.MasterCourseId == topic.MasterCourseId &&
                t.SubCourseId == topic.SubCourseId &&
                t.TopicName == topic.TopicName);

            if (exists)
            {
                return "Topic already exists.";
            }

            existing.MasterCourseId = topic.MasterCourseId;

            existing.SubCourseId = topic.SubCourseId;

            existing.TopicName = topic.TopicName;

            existing.VideoUrl = topic.VideoUrl;

            existing.Status = topic.Status;

            if (thumbnail != null)
            {
                existing.ThumbnailPath = SaveThumbnail(thumbnail);
            }

            _context.SaveChanges();

            return "Topic updated successfully!";
        }

        public string Delete(int id)
        {
            var topic = _context.Topics.Find(id);

            if (topic == null)
            {
                return "Topic not found.";
            }

            _context.Topics.Remove(topic);

            _context.SaveChanges();

            return "Topic deleted successfully!";
        }

        private string SaveThumbnail(IFormFile thumbnail)
        {
            string fileName = Guid.NewGuid() + Path.GetExtension(thumbnail.FileName);

            string folderPath = Path.Combine(_env.WebRootPath, "uploads");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fullPath =  Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                thumbnail.CopyTo(stream);
            }

            return "/uploads/" + fileName;
        }
    }
} 