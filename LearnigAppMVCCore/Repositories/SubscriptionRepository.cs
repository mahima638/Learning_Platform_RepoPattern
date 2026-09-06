using LearnigAppMVCCore.Data;
using LearnigAppMVCCore.Interfaces;
using LearnigAppMVCCore.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnigAppMVCCore.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly SubscriptionContext db;

        public SubscriptionRepository(SubscriptionContext context)
        {
            db = context;
        }

        // Get all subscriptions
        public List<Subscriptions> GetSubscriptions()
        {
            return db.Subscriptions
                .Include(x => x.MasterCourse)
                .Include(x => x.SubscriptionSubCourses)
                .ThenInclude(x => x.SubCourse)
                .ToList();
        }

        // Get active master courses
        public List<MasterCourse> GetActiveMasterCourses()
        {
            return db.MasterCourses
                .Where(x => x.mstatus == "Active")
                .ToList();
        }

        // Get sub courses
        public List<SubCourse> GetSubCourses(int mid)
        {
            return db.SubCourses
                .Where(x => x.mid == mid && x.sstatus == "Active")
                .ToList();
        }

        // Get subscription by ID
        public Subscriptions GetSubscriptionById(int id)
        {
            return db.Subscriptions
                .Include(x => x.MasterCourse)
                .Include(x => x.SubscriptionSubCourses)
                .ThenInclude(x => x.SubCourse)
                .FirstOrDefault(x => x.sub_id == id);
        }

        // Add subscription
        public async Task AddSubscriptionAsync(
            Subscriptions subscription)
        {
            db.Subscriptions.Add(subscription);

            await db.SaveChangesAsync();
        }

        // Update subscription
        public async Task UpdateSubscriptionAsync(
            Subscriptions subscription)
        {
            db.Subscriptions.Update(subscription);

            await db.SaveChangesAsync();
        }

        // Delete subscription
        public async Task DeleteSubscriptionAsync(int id)
        {
            var subscription = await db.Subscriptions
                .FirstOrDefaultAsync(x => x.sub_id == id);

            if (subscription != null)
            {
                db.Subscriptions.Remove(subscription);

                await db.SaveChangesAsync();
            }
        }

        // Get selected sub courses
        public List<SubscriptionSubCourse> GetSubscriptionSubCourses(
            int subId)
        {
            return db.SubscriptionSubCourses
                .Where(x => x.sub_id == subId)
                .ToList();
        }

        // Add subscription-subcourse
        public async Task AddSubscriptionSubCourseAsync(
            SubscriptionSubCourse subscriptionSubCourse)
        {
            db.SubscriptionSubCourses.Add(
                subscriptionSubCourse);

            await db.SaveChangesAsync();
        }

        // Remove old sub courses
        public async Task RemoveSubscriptionSubCoursesAsync(
            int subId)
        {
            var oldSubCourses = await db.SubscriptionSubCourses
                .Where(x => x.sub_id == subId)
                .ToListAsync();

            if (oldSubCourses.Any())
            {
                db.SubscriptionSubCourses.RemoveRange(
                    oldSubCourses);

                await db.SaveChangesAsync();
            }
        }
    }
}