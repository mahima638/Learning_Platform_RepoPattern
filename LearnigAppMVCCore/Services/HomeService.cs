using LearnigAppMVCCore.Interfaces;
using LearnigAppMVCCore.Models;

namespace LearnigAppMVCCore.Services
{
    public class HomeService : IHomeService
    {
        private readonly ISubscriptionRepository _repository;

        public HomeService(ISubscriptionRepository repository)
        {
            _repository = repository;
        }

        public List<Subscriptions> GetSubscriptions()
        {
            return _repository.GetSubscriptions();
        }

        public List<MasterCourse> GetActiveMasterCourses()
        {
            return _repository.GetActiveMasterCourses();
        }

        public List<SubCourse> GetSubCourses(int mid)
        {
            return _repository.GetSubCourses(mid);
        }

        public Subscriptions GetSubscriptionById(int id)
        {
            return _repository.GetSubscriptionById(id);
        }

        public async Task AddSubscriptionAsync(
            Subscriptions subscription)
        {
            await _repository.AddSubscriptionAsync(subscription);
        }

        public async Task UpdateSubscriptionAsync(
            Subscriptions subscription)
        {
            await _repository.UpdateSubscriptionAsync(subscription);
        }

        public async Task DeleteSubscriptionAsync(int id)
        {
            await _repository.DeleteSubscriptionAsync(id);
        }

        public List<SubscriptionSubCourse> GetSubscriptionSubCourses(
            int subId)
        {
            return _repository.GetSubscriptionSubCourses(subId);
        }

        public async Task AddSubscriptionSubCourseAsync(
            SubscriptionSubCourse subscriptionSubCourse)
        {
            await _repository.AddSubscriptionSubCourseAsync(
                subscriptionSubCourse);
        }

        public async Task RemoveSubscriptionSubCoursesAsync(int subId)
        {
            await _repository.RemoveSubscriptionSubCoursesAsync(subId);
        }
    }
}