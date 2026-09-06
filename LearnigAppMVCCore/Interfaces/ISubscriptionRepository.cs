using LearnigAppMVCCore.Models;

namespace LearnigAppMVCCore.Interfaces
{
    public interface ISubscriptionRepository
    {
        List<Subscriptions> GetSubscriptions();

        List<MasterCourse> GetActiveMasterCourses();

        List<SubCourse> GetSubCourses(int mid);

        Subscriptions GetSubscriptionById(int id);

        Task AddSubscriptionAsync(Subscriptions subscription);

        Task DeleteSubscriptionAsync(int id);

        Task UpdateSubscriptionAsync(Subscriptions subscription);

        List<SubscriptionSubCourse> GetSubscriptionSubCourses(int subId);

        Task AddSubscriptionSubCourseAsync(
            SubscriptionSubCourse subscriptionSubCourse);

        Task RemoveSubscriptionSubCoursesAsync(int subId);
    }
}