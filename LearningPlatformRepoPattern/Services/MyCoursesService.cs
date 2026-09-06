using LearningPlatformRepoPattern.Data;
using LearningPlatformRepoPattern.Models;
using LearningPlatformRepoPattern.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LearningPlatformRepoPattern.Services
{
    public class MyCoursesService : IMyCoursesRepository
    {
        private readonly ApplicationDbContext _context;

        public MyCoursesService(ApplicationDbContext context)
        {
            _context = context;
        }

       //my courses
        public async Task<List<MyCourseViewModel>> GetMyCourses(int userId)
        {
            var myCourses = await _context.MyCourses
                .Where(x => x.UserId == userId)
                .Include(x => x.SubCourse)
                .ThenInclude(x => x.MasterCourse)
                .ToListAsync();

            var courses = myCourses.Select(x => new MyCourseViewModel
            {
                Sid = x.SubCourse.Id,
                UserId = x.UserId,
                Sname = x.SubCourse.SubCourseName,
                Sstatus = x.SubCourse.Status,
                Samount = x.SubCourse.Amount,
                Mname = x.SubCourse.MasterCourse.CourseName,
                Mthumbnail = x.SubCourse.MasterCourse.ThumbnailPath
            }).ToList();

            return courses;
        }

        // =====================================================
        // WATCH VIDEO
        // =====================================================

        public async Task<WatchVideoViewModel> GetWatchVideo(int sid,int userId,int? tid)
        {
            // Get Sub Course
            var subCourse = await _context.SubCourses
                .FirstOrDefaultAsync(x => x.Id == sid);

            if (subCourse == null)
            {
                return null;
            }

            // Get Master Course
            var masterCourse = await _context.MasterCourses
                .FirstOrDefaultAsync(x => x.Id == subCourse.MasterCourseId);

            if (masterCourse == null)
            {
                return null;
            }

            // Get Topics
            var topics = await _context.Topics
                .Where(x => x.SubCourseId == sid)
                .OrderBy(x => x.Id)
                .ToListAsync();

            if (topics.Count == 0)
            {
                return null;
            }

            // Get User Progress
            var progress = await _context.TopicProgress
                .Where(x =>
                    x.UserId == userId &&
                    x.Sid == sid)
                .ToListAsync();

            // Create Topic Items
            var topicItems = new List<TopicItemViewModel>();

            for (int i = 0; i < topics.Count; i++)
            {
                var topic = topics[i];
                var currentProgress = progress.FirstOrDefault(
                    x => x.Tid == topic.Id);

                bool isCompleted =
                    currentProgress != null &&
                    currentProgress.McqPassed;
                bool isUnlocked;

                // First topic is always unlocked
                if (i == 0)
                {
                    isUnlocked = true;
                }
                else
                {
                    var previousTopic = topics[i - 1];

                    var previousProgress = progress.FirstOrDefault(
                        x => x.Tid == previousTopic.Id);

                    isUnlocked =
                        previousProgress != null &&
                        previousProgress.McqPassed;
                }

                topicItems.Add(new TopicItemViewModel
                {
                    Topic = topic,
                    IsUnlocked = isUnlocked,
                    IsCompleted = isCompleted
                });
            }

           //select Current topic
            int currentTopicId;

            if (tid.HasValue)
            {
                var selectedTopic = topicItems.FirstOrDefault(
                    x => x.Topic.Id == tid.Value);

                if (selectedTopic == null ||
                    !selectedTopic.IsUnlocked)
                {
                    return null;
                }

                currentTopicId = tid.Value;
            }
            else
            {
                currentTopicId = topicItems
                    .First(x => x.IsUnlocked)
                    .Topic
                    .Id;
            }

           //get materil
            var material = await _context.Materials
                .FirstOrDefaultAsync(x =>
                    x.SubCourseId == sid &&
                    x.TopicId == currentTopicId);

            //Get mcq

            var mcqs = new List<Mcq>();

            if (material != null)
            {
                mcqs = await _context.Mcqs
                    .Where(x => x.MaterialId == material.Id)
                    .OrderBy(x => x.Id)
                    .Take(3)
                    .ToListAsync();
            }

           // certificate check
            bool certificateUnlocked =
                topicItems.Count > 0 &&
                topicItems.All(x => x.IsCompleted);

           //view model
            var model = new WatchVideoViewModel
            {
                Sid = sid,
                UserId = userId,
                CurrentTopicId = currentTopicId,

<<<<<<< HEAD
            // Create ViewModel
            var model =
                new WatchVideoViewModel
                {
                    Sid = sid,
                    UserId = userId,
                    CurrentTopicId = currentTopicId,
                    SubCourseName = subCourse.SubCourseName,
                    CourseName = masterCourse.CourseName,
                    Mthumbnail = masterCourse.ThumbnailPath,
                    TopicItems = topicItems,
                    Mcqs = mcqs,
                    CertificateUnlocked =
                        certificateUnlocked
                };
=======
                Sname = subCourse.SubCourseName,
                Mname = masterCourse.CourseName,
                Mthumbnail = masterCourse.ThumbnailPath,

                TopicItems = topicItems,
                Mcqs = mcqs,

                CertificateUnlocked = certificateUnlocked
            };
>>>>>>> origin/main

            return model;
        }

       //assignment download
        public async Task<(byte[] FileBytes, string FileName)>
            DownloadAssignment(int sid)
        {
            var material = await _context.Materials
                .FirstOrDefaultAsync(x => x.SubCourseId == sid);

            if (material == null)
            {
                return (null, null);
            }

            if (string.IsNullOrEmpty(material.Assignment))
            {
                return (null, null);
            }

            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                material.Assignment.TrimStart('/'));

            if (!File.Exists(filePath))
            {
                return (null, null);
            }

            var fileBytes = await File.ReadAllBytesAsync(filePath);

            var fileName = Path.GetFileName(filePath);

            return (fileBytes, fileName);
        }

       //submit assignment
        public async Task<bool> SubmitAssignment(
            IFormFile assignmentFile)
        {
            if (assignmentFile == null ||
                assignmentFile.Length == 0)
            {
                return false;
            }

            var folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "submissions");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName =
                Guid.NewGuid().ToString() +
                Path.GetExtension(assignmentFile.FileName);

            var filePath = Path.Combine(
                folderPath,
                fileName);

            using (var stream = new FileStream(
                filePath,
                FileMode.Create))
            {
                await assignmentFile.CopyToAsync(stream);
            }

            return true;
        }

        //submit mcq
        public async Task<(int Score, int Total, bool Passed, string Message)>
            SubmitMcq(
                int sid,
                int userId,
                int tid,
                List<string> answers)
        {
            // Get Topics
            var topics = await _context.Topics
                .Where(x => x.SubCourseId == sid)
                .OrderBy(x => x.Id)
                .ToListAsync();

            // Find Current Topic
            var topic = topics.FirstOrDefault(
                x => x.Id == tid);

            if (topic == null)
            {
                return (
                    0,
                    0,
                    false,
                    "Topic not found."
                );
            }

            // Find Topic Index
            int topicIndex = topics.FindIndex(
                x => x.Id == tid);

           //check previous topic
            if (topicIndex > 0)
            {
                int previousTopicId =
                    topics[topicIndex - 1].Id;

                bool previousPassed =
                    await _context.TopicProgress.AnyAsync(x =>
                        x.UserId == userId &&
                        x.Sid == sid &&
                        x.Tid == previousTopicId &&
                        x.McqPassed);

                if (!previousPassed)
                {
                    return (
                        0,
                        0,
                        false,
                        "Previous topic is not completed."
                    );
                }
            }

            //Get material

            var material = await _context.Materials
                .FirstOrDefaultAsync(x =>
                    x.SubCourseId == sid &&
                    x.TopicId == tid);

            if (material == null)
            {
                return (
                    0,
                    0,
                    false,
                    "Material not found."
                );
            }

           //get mcq
            var mcqs = await _context.Mcqs
                .Where(x => x.MaterialId == material.Id)
                .OrderBy(x => x.Id)
                .Take(3)
                .ToListAsync();

            if (mcqs.Count == 0)
            {
                return (
                    0,
                    0,
                    false,
                    "No MCQ questions found."
                );
            }

            //calculate score
            int score = 0;

            for (int i = 0; i < mcqs.Count; i++)
            {
                if (answers != null &&
                    i < answers.Count)
                {
                    if (string.Equals(
                        answers[i]?.Trim(),
                        mcqs[i].Answer?.Trim(),
                        StringComparison.OrdinalIgnoreCase))
                    {
                        score++;
                    }
                }
            }

            // Pass only when all 3 MCQs are correct
            bool passed =
                score == 3 &&
                mcqs.Count == 3;

            //save progress

            if (passed)
            {
                var existingProgress =
                    await _context.TopicProgress
                        .FirstOrDefaultAsync(x =>
                            x.UserId == userId &&
                            x.Sid == sid &&
                            x.Tid == tid);

                if (existingProgress == null)
                {
                    var progress = new TopicProgress
                    {
                        UserId = userId,
                        Sid = sid,
                        Tid = tid,
                        McqPassed = true,
                        CompletedAt = DateTime.Now
                    };

                    _context.TopicProgress.Add(progress);
                }
                else
                {
                    existingProgress.McqPassed = true;
                    existingProgress.CompletedAt = DateTime.Now;

                    _context.TopicProgress.Update(
                        existingProgress);
                }

                await _context.SaveChangesAsync();
            }

            //result message
            string message;

            if (passed)
            {
                message =
                    "Congratulations! You scored 3/3. Next topic is unlocked.";
            }
            else
            {
                message =
                    "You scored " +
                    score +
                    "/" +
                    mcqs.Count +
                    ". You must score 3/3 to unlock the next topic.";
            }

            return (
                score,
                mcqs.Count,
                passed,
                message);
        }

        //certificate

        public async Task<CertificateViewModel>
            GetCertificate(
                int sid,
                int userId)
        {
            // Get Logged-in User
            var user = await _context.Users
                .FirstOrDefaultAsync(
                    x => x.UserId == userId);

            if (user == null)
            {
                return null;
            }

            // Get Selected Sub Course
            var subCourse = await _context.SubCourses
                .FirstOrDefaultAsync(
                    x => x.Id == sid);

            if (subCourse == null)
            {
                return null;
            }

            // Get Master Course
            var masterCourse = await _context.MasterCourses
                .FirstOrDefaultAsync(
                    x => x.Id == subCourse.MasterCourseId);

            if (masterCourse == null)
            {
                return null;
            }

            // Get All Topics
            var topics = await _context.Topics
                .Where(x => x.SubCourseId == sid)
                .OrderBy(x => x.Id)
                .ToListAsync();

            if (topics.Count == 0)
            {
                return null;
            }

            // Get User Progress
            var progress = await _context.TopicProgress
                .Where(x =>
                    x.UserId == userId &&
                    x.Sid == sid)
                .ToListAsync();

            // Get Completed Topic IDs
            var completedTopicIds = progress
                .Where(x => x.McqPassed)
                .Select(x => x.Tid)
                .Distinct()
                .ToList();

            // Check All Topics Completed
            bool allTopicsCompleted = topics.All(
                topic => completedTopicIds.Contains(topic.Id));

            if (!allTopicsCompleted)
            {
                return null;
            }

            //create certificate

            var model = new CertificateViewModel
            {
                UserName = user.UserName,
                CourseName = masterCourse.CourseName,
                SubCourseName = subCourse.SubCourseName,
                CompletionDate = DateTime.Now
            };

            return model;
        }
    }
}