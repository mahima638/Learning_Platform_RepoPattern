using LearningPlatformRepoPattern.Models;

namespace LearningPlatformRepoPattern.Repository
{
    public interface IMyCoursesRepository
    {
        Task<List<MyCourseViewModel>> GetMyCourses(int userId);

        Task<SubCourse> GetSubCourse(int sid);

        Task<MasterCourse> GetMasterCourse(int mid);

        Task<List<Topic>> GetTopics(int sid);

        Task<List<TopicProgress>> GetTopicProgress(
            int userId,
            int sid);

        Task<Material> GetMaterial(
            int sid,
            int tid);

        Task<Material> GetAssignment(int sid);

        Task<List<Mcq>> GetMcqs(int materialId);

        Task<TopicProgress> GetTopicProgress(
            int userId,
            int sid,
            int tid);

        Task AddTopicProgress(
            TopicProgress progress);

        Task UpdateTopicProgress(
            TopicProgress progress);

        Task<bool> IsTopicPassed(
            int userId,
            int sid,
            int tid);

        Task<User> GetUser(int userId);

        Task<int> GetCompletedTopics(
            int userId,
            int sid);
    }
}