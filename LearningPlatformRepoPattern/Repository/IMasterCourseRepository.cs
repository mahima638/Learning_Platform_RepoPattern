using LearningPlatformRepoPattern.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace LearningPlatformRepoPattern.Repository
{
    public interface IMasterCourseRepository
    {
        List<MasterCourse> GetAll();
        MasterCourse GetById(int id);
        List<MasterCourse> GetActiveCourses();
        string Add(MasterCourse course, IFormFile thumbnail, string createdBy);
        string Update(MasterCourse course, IFormFile thumbnail);
        string Delete(int id);
    }
}