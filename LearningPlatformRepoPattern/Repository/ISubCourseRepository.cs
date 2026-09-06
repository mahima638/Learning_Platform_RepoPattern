using LearningPlatformRepoPattern.Models;

namespace LearningPlatformRepoPattern.Repository
{
    public interface ISubCourseRepository
    {
        List<SubCourse> GetAll();
        SubCourse GetById(int id);
        string Add(SubCourse subCourse, string createdBy);
        string Update(SubCourse subCourse);
        string Delete(int id);
    }
}
