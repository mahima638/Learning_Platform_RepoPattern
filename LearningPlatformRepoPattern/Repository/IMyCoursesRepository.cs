using LearningPlatformRepoPattern.Models;
using Microsoft.AspNetCore.Http;

namespace LearningPlatformRepoPattern.Repository
{
    public interface IMyCoursesRepository
    {
        //to show my courses on page 
        Task<List<MyCourseViewModel>> GetMyCourses(
            int userId);


        //for watch video
        Task<WatchVideoViewModel> GetWatchVideo(
            int sid,
            int userId,
            int? tid);


        //download assignment
        Task<(byte[] FileBytes, string FileName)>
            DownloadAssignment(int sid);

        //submit Assignment
        Task<bool> SubmitAssignment(
            IFormFile assignmentFile);

        //Mcq
        Task<(int Score,
              int Total,
              bool Passed,
              string Message)>
            SubmitMcq(
                int sid,
                int userId,
                int tid,
                List<string> answers);


        //certificate
        Task<CertificateViewModel> GetCertificate(
            int sid,
            int userId);
        
    }
}