using LearningPlatformRepoPattern.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace LearningPlatformRepoPattern.Repository
{
    public interface ITopicRepository
    {
        List<Topic> GetAll();
        Topic GetById(int id);
        List<MasterCourse> GetMasterCourses();
        List<SubCourse> GetSubCourses(int masterCourseId);
        string Add(Topic topic, IFormFile thumbnail, string createdBy);
        string Update(Topic topic, IFormFile thumbnail);
        string Delete(int id);
    }
}