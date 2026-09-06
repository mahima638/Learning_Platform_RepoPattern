using LearningPlatformRepoPattern.Data;
using LearningPlatformRepoPattern.Models;
using LearningPlatformRepoPattern.Repository;
using Microsoft.EntityFrameworkCore;

namespace LearningPlatformRepoPattern.Services
{
    public class SubCourseService : ISubCourseRepository
    {
        private readonly ApplicationDbContext _context;

        public SubCourseService(ApplicationDbContext context)
        {
            _context = context;
        }
        public string Add(SubCourse subCourse, string createdBy)
        {
            bool alreadyExists = _context.SubCourses.Any(s =>
                s.MasterCourseId == subCourse.MasterCourseId &&
                s.SubCourseName.ToLower() == subCourse.SubCourseName.ToLower());

            if (alreadyExists)
            {
                return "This Sub Course already exists under the selected Master Course!";
            }

            subCourse.CreatedAt = DateTime.Now;
            subCourse.CreatedBy = "Admin";

            _context.SubCourses.Add(subCourse);
            _context.SaveChanges();

            return "Sub Course added successfully!";
        }

        public string Delete(int id)
        {
            var subCourse = _context.SubCourses.Find(id);
            if (subCourse == null)
            {
                return "Sub Course not found.";
            }

            _context.SubCourses.Remove(subCourse);
            _context.SaveChanges();

            return "Sub Course deleted successfully!";
        }

        public List<SubCourse> GetAll()
        {
            return _context.SubCourses
                .Include(s => s.MasterCourse)
                .OrderByDescending(s => s.CreatedAt)
                .ToList();
        }

        public SubCourse GetById(int id)
        {
            return _context.SubCourses.Find(id);
        }

        public string Update(SubCourse subCourse)
        {
            var existing = _context.SubCourses.Find(subCourse.Id);
            if (existing == null)
            {
                return "Sub Course not found.";
            }

            bool duplicateExists = _context.SubCourses.Any(s =>
                s.Id != subCourse.Id &&
                s.MasterCourseId == subCourse.MasterCourseId &&
                s.SubCourseName.ToLower() == subCourse.SubCourseName.ToLower());

            if (duplicateExists)
            {
                return "Another Sub Course with this name already exists under the selected Master Course!";
            }

            existing.MasterCourseId = subCourse.MasterCourseId;
            existing.SubCourseName = subCourse.SubCourseName;
            existing.Amount = subCourse.Amount;
            existing.Status = subCourse.Status;

            _context.SaveChanges();

            return "Sub Course updated successfully!";
        }
    }
}
