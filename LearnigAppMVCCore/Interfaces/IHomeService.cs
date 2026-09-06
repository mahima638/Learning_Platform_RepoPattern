using LearnigAppMVCCore.Models;

namespace LearnigAppMVCCore.Interfaces
{
    public interface IHomeService
    {
        List<Subscriptions> GetSubscriptions();

        List<MasterCourse> GetActiveMasterCourses();

        List<SubCourse> GetSubCourses(int mid);

        Subscriptions GetSubscriptionById(int id);

        Task AddSubscriptionAsync(Subscriptions subscription);

        Task UpdateSubscriptionAsync(Subscriptions subscription);

        Task DeleteSubscriptionAsync(int id);

        List<SubscriptionSubCourse> GetSubscriptionSubCourses(int subId);

        Task AddSubscriptionSubCourseAsync(
            SubscriptionSubCourse subscriptionSubCourse);

        Task RemoveSubscriptionSubCoursesAsync(int subId);
    }
}