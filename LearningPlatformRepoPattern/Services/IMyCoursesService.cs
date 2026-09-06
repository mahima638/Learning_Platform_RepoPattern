using LearningPlatformRepoPattern.Models;
using Microsoft.AspNetCore.Http;

namespace LearningPlatformRepoPattern.Services
{
    public interface IMyCoursesService
    {
        Task<List<MyCourseViewModel>> GetMyCourses(
            int userId);

        Task<WatchVideoViewModel> GetWatchVideo(
            int sid,
            int userId,
            int? tid);

        Task<(byte[] FileBytes, string FileName)> DownloadAssignment(int sid);

        Task<bool> SubmitAssignment(
            IFormFile assignmentFile);

        Task<(int Score, int Total, bool Passed, string Message)> SubmitMcq(
            int sid,
            int userId,
            int tid,
            List<string> answers);

        Task<CertificateViewModel> GetCertificate(
            int sid,
            int userId);
    }
}