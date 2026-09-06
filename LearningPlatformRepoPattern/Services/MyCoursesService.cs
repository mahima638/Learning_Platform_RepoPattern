using LearningPlatformRepoPattern.Models;
using LearningPlatformRepoPattern.Repository;
using Microsoft.AspNetCore.Http;

namespace LearningPlatformRepoPattern.Services
{
    public class MyCoursesService : IMyCoursesService
    {
        private readonly IMyCoursesRepository _repository;

        public MyCoursesService(
            IMyCoursesRepository repository)
        {
            _repository = repository;
        }

        // My Courses
        public async Task<List<MyCourseViewModel>> GetMyCourses(
            int userId)
        {
            return await _repository.GetMyCourses(userId);
        }


        // Watch Video
        public async Task<WatchVideoViewModel> GetWatchVideo(
            int sid,
            int userId,
            int? tid)
        {
            // Get Sub Course
            var subCourse =
                await _repository.GetSubCourse(sid);

            if (subCourse == null)
            {
                return null;
            }

            // Get Master Course
            var masterCourse =
                await _repository.GetMasterCourse(
                    subCourse.MasterCourseId);

            if (masterCourse == null)
            {
                return null;
            }

            // Get Topics
            var topics =
                await _repository.GetTopics(sid);

            if (topics.Count == 0)
            {
                return null;
            }

            // Get Progress
            var progress =
                await _repository.GetTopicProgress(
                    userId,
                    sid);

            // Create Topic Items
            var topicItems =
                new List<TopicItemViewModel>();

            for (int i = 0; i < topics.Count; i++)
            {
                var topic = topics[i];

                var currentProgress =
                    progress.FirstOrDefault(
                        x => x.Tid == topic.Id);

                bool isCompleted =
                    currentProgress != null &&
                    currentProgress.McqPassed;

                bool isUnlocked;

                // First Topic
                if (i == 0)
                {
                    isUnlocked = true;
                }
                else
                {
                    var previousTopic =
                        topics[i - 1];

                    var previousProgress =
                        progress.FirstOrDefault(
                            x => x.Tid == previousTopic.Id);

                    isUnlocked =
                        previousProgress != null &&
                        previousProgress.McqPassed;
                }

                topicItems.Add(
                    new TopicItemViewModel
                    {
                        Topic = topic,
                        IsUnlocked = isUnlocked,
                        IsCompleted = isCompleted
                    });
            }


            // Select Current Topic
            int currentTopicId;

            if (tid.HasValue)
            {
                var selectedTopic =
                    topicItems.FirstOrDefault(
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
                currentTopicId =
                    topicItems
                        .First(x => x.IsUnlocked)
                        .Topic.Id;
            }


            // Get Material
            var material =
                await _repository.GetMaterial(
                    sid,
                    currentTopicId);


            // Get MCQs
            var mcqs =
                new List<Mcq>();

            if (material != null)
            {
                mcqs =
                    await _repository.GetMcqs(
                        material.Id);
            }


            // Certificate Check
            bool certificateUnlocked =
                topicItems.All(
                    x => x.IsCompleted);


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

            return model;
        }


        // Download Assignment
        public async Task<(byte[] FileBytes, string FileName)> DownloadAssignment(int sid)
        {
            var material = await _repository.GetAssignment(sid);

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
                material.Assignment.TrimStart('/')
            );

            if (!File.Exists(filePath))
            {
                return (null, null);
            }

            var fileBytes = await File.ReadAllBytesAsync(filePath);

            var fileName = Path.GetFileName(filePath);

            return (fileBytes, fileName);
        }
        // Submit Assignment
        public async Task<bool> SubmitAssignment(
            IFormFile assignmentFile)
        {
            if (assignmentFile == null ||
                assignmentFile.Length == 0)
            {
                return false;
            }

            var folderPath =
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "submissions");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName =
                Guid.NewGuid().ToString()
                + Path.GetExtension(
                    assignmentFile.FileName);

            var filePath =
                Path.Combine(
                    folderPath,
                    fileName);

            using (var stream =
                   new FileStream(
                       filePath,
                       FileMode.Create))
            {
                await assignmentFile.CopyToAsync(
                    stream);
            }

            return true;
        }


        // Submit MCQ
        public async Task<(int Score, int Total, bool Passed, string Message)> SubmitMcq(
            int sid,
            int userId,
            int tid,
            List<string> answers)
        {
            // Get Topics
            var topics =
                await _repository.GetTopics(sid);

            // Find Current Topic
            var topic =
                topics.FirstOrDefault(
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
            int topicIndex =
                topics.FindIndex(
                    x => x.Id == tid);

            // Check Previous Topic
            if (topicIndex > 0)
            {
                int previousTopicId =
                    topics[topicIndex - 1].Id;

                bool previousPassed =
                    await _repository.IsTopicPassed(
                        userId,
                        sid,
                        previousTopicId);

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

            // Get Material
            var material =
                await _repository.GetMaterial(
                    sid,
                    tid);

            if (material == null)
            {
                return (
                    0,
                    0,
                    false,
                    "Material not found."
                );
            }

            // Get MCQs
            var mcqs =
                await _repository.GetMcqs(
                    material.Id);

            if (mcqs.Count == 0)
            {
                return (
                    0,
                    0,
                    false,
                    "No MCQ questions found."
                );
            }

            // Calculate Score
            int score = 0;

            for (int i = 0;
                 i < mcqs.Count;
                 i++)
            {
                if (answers != null &&
                    i < answers.Count)
                {
                    if (string.Equals(
                        answers[i]?.Trim(),
                        mcqs[i].Answer?.Trim(),
                        StringComparison
                            .OrdinalIgnoreCase))
                    {
                        score++;
                    }
                }
            }

            // Pass only if 3/3
            bool passed =
                score == 3 &&
                mcqs.Count == 3;


            // Save Progress
            if (passed)
            {
                var existingProgress =
                    await _repository.GetTopicProgress(
                        userId,
                        sid,
                        tid);

                if (existingProgress == null)
                {
                    var progress =
                        new TopicProgress
                        {
                            UserId = userId,
                            Sid = sid,
                            Tid = tid,
                            McqPassed = true,
                            CompletedAt =
                                DateTime.Now
                        };

                    await _repository
                        .AddTopicProgress(progress);
                }
                else
                {
                    existingProgress.McqPassed =
                        true;

                    existingProgress.CompletedAt =
                        DateTime.Now;

                    await _repository
                        .UpdateTopicProgress(
                            existingProgress);
                }
            }

            string message;

            if (passed)
            {
                message =
                    "Congratulations! You scored 3/3. Next topic is unlocked.";
            }
            else
            {
                message =
                    "You scored "
                    + score
                    + "/3. You must score 3/3 to unlock the next topic.";
            }

            return (
                score,
                mcqs.Count,
                passed,
                message
            );
        }


        // Certificate
        public async Task<CertificateViewModel> GetCertificate(
            int sid,
            int userId)
        {
            // Get User
            var user =
                await _repository.GetUser(userId);

            if (user == null)
            {
                return null;
            }

            // Get Sub Course
            var subCourse =
                await _repository.GetSubCourse(sid);

            if (subCourse == null)
            {
                return null;
            }

            // Get Master Course
            var masterCourse =
                await _repository.GetMasterCourse(
                    subCourse.Id);

            if (masterCourse == null)
            {
                return null;
            }

            // Get Topics
            var topics =
                await _repository.GetTopics(sid);

            // Get Completed Topics
            var completedTopics =
                await _repository.GetCompletedTopics(
                    userId,
                    sid);

            if (topics.Count == 0 ||
                completedTopics < topics.Count)
            {
                return null;
            }

            // Create Certificate
            var model =
                new CertificateViewModel
                {
                    UserName = user.UserName,
                    CourseName =
                        masterCourse.CourseName,
                    SubCourseName =
                        subCourse.SubCourseName,
                    CompletionDate =
                        DateTime.Now
                };

            return model;
        }
    }
}