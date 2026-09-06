using LearningPlatformRepoPattern.Data;
using LearningPlatformRepoPattern.Models;
using Microsoft.EntityFrameworkCore;

namespace LearningPlatformRepoPattern.Repository
{
    public class MyCoursesRepository : IMyCoursesRepository
    {
        private readonly ApplicationDbContext _context;

        public MyCoursesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get My Courses
        public async Task<List<MyCourseViewModel>> GetMyCourses(int userId)
        {
            var courses = await _context.MyCourses
                .Where(x => x.UserId == userId)
                .Join(
                    _context.SubCourses,
                    mc => mc.Sid,
                    sc => sc.Id,
                    (mc, sc) => new { mc, sc }
                )
                .Join(
                    _context.MasterCourses,
                    x => x.sc.MasterCourseId,
                    master => master.Id,
                    (x, master) => new MyCourseViewModel
                    {
                        Sid = x.sc.Id,
                        UserId = x.mc.UserId,
                        Sname = x.sc.SubCourseName,
                        Sstatus = x.sc.Status,
                        Samount = x.sc.Amount,
                        Mname = master.CourseName,
                        Mthumbnail = master.ThumbnailPath
                    }
                )
                .ToListAsync();

            return courses;
        }

        // Get Sub Course
        public async Task<SubCourse> GetSubCourse(int sid)
        {
            return await _context.SubCourses
                .FirstOrDefaultAsync(x => x.Id == sid);
        }

        // Get Master Course
        public async Task<MasterCourse> GetMasterCourse(int mid)
        {
            return await _context.MasterCourses
                .FirstOrDefaultAsync(x => x.Id == mid);
        }

        // Get Topics
        public async Task<List<Topic>> GetTopics(int sid)
        {
            return await _context.Topics
                .Where(x => x.SubCourseId == sid)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }
        // Get Topic Progress
        public async Task<List<TopicProgress>> GetTopicProgress(
            int userId,
            int sid)
        {
            return await _context.TopicProgress
                .Where(x =>
                    x.UserId == userId &&
                    x.Sid == sid)
                .ToListAsync();
        }

        // Get Material
        public async Task<Material> GetMaterial(
            int sid,
            int tid)
        {
            return await _context.Materials
                .FirstOrDefaultAsync(x =>
                    x.SubCourseId == sid &&
                    x.TopicId == tid);
        }

        // Get Assignment
        public async Task<Material> GetAssignment(int sid)
        {
            return await _context.Materials
                .FirstOrDefaultAsync(x =>
                    x.SubCourseId == sid);
        }

        // Get MCQs
        public async Task<List<Mcq>> GetMcqs(int materialId)
        {
            return await _context.Mcqs
                .Where(x => x.MaterialId == materialId)
                .OrderBy(x => x.Id)
                .Take(3)
                .ToListAsync();
        }

        // Get Existing Topic Progress
        public async Task<TopicProgress> GetTopicProgress(
            int userId,
            int sid,
            int tid)
        {
            return await _context.TopicProgress
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.Sid == sid &&
                    x.Tid == tid);
        }

        // Add Progress
        public async Task AddTopicProgress(
            TopicProgress progress)
        {
            _context.TopicProgress.Add(progress);

            await _context.SaveChangesAsync();
        }

        // Update Progress
        public async Task UpdateTopicProgress(
            TopicProgress progress)
        {
            _context.TopicProgress.Update(progress);

            await _context.SaveChangesAsync();
        }

        // Check Previous Topic Passed
        public async Task<bool> IsTopicPassed(
            int userId,
            int sid,
            int tid)
        {
            return await _context.TopicProgress
                .AnyAsync(x =>
                    x.UserId == userId &&
                    x.Sid == sid &&
                    x.Tid == tid &&
                    x.McqPassed);
        }

        // Get User
        public async Task<User> GetUser(int userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId);
        }

        // Get Completed Topics
        public async Task<int> GetCompletedTopics(
            int userId,
            int sid)
        {
            return await _context.TopicProgress
                .Where(x =>
                    x.UserId == userId &&
                    x.Sid == sid &&
                    x.McqPassed)
                .CountAsync();
        }
    }
}