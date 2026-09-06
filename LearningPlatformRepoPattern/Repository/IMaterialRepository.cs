using LearningPlatformRepoPattern.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace LearningPlatformRepoPattern.Repository
{
    public interface IMaterialRepository
    {
        List<Material> GetAll();
        Material GetById(int id);
        List<MasterCourse> GetMasterCourses();
        List<SubCourse> GetSubCourses(int masterCourseId);
        List<Topic> GetTopics(int subCourseId);

        string Add(Material material, IFormFile assignmentFile, List<Mcq> mcqs);
        string Update(Material material, IFormFile assignmentFile, List<Mcq> mcqs);
        string Delete(int id);
    }
}