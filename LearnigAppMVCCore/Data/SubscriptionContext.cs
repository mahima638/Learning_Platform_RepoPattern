using LearnigAppMVCCore.Models;
using Microsoft.EntityFrameworkCore;

namespace LearnigAppMVCCore.Data
{
    public class SubscriptionContext : DbContext
    {
        public SubscriptionContext(
            DbContextOptions<SubscriptionContext> options)
            : base(options)
        {
        }

        public DbSet<Subscriptions> Subscriptions { get; set; }

        public DbSet<MasterCourse> MasterCourses { get; set; }

        public DbSet<SubCourse> SubCourses { get; set; }

        public DbSet<SubscriptionSubCourse> SubscriptionSubCourses { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // -----------------------------------------
            // Subscriptions -> MasterCourse
            // Subscriptions.mid -> MasterCourse.mid
            // -----------------------------------------

            modelBuilder.Entity<Subscriptions>()
                .HasOne(s => s.MasterCourse)
                .WithMany()
                .HasForeignKey(s => s.mid)
                .HasPrincipalKey(m => m.mid)
                .OnDelete(DeleteBehavior.NoAction);


            // -----------------------------------------
            // SubCourse -> MasterCourse
            // SubCourse.mid -> MasterCourse.mid
            // -----------------------------------------

            modelBuilder.Entity<SubCourse>()
                .HasOne<MasterCourse>()
                .WithMany()
                .HasForeignKey(sc => sc.mid)
                .HasPrincipalKey(mc => mc.mid)
                .OnDelete(DeleteBehavior.NoAction);


            // -----------------------------------------
            // SubscriptionSubCourse -> Subscriptions
            // SubscriptionSubCourse.sub_id -> Subscriptions.sub_id
            // -----------------------------------------

            modelBuilder.Entity<SubscriptionSubCourse>()
                .HasOne(ssc => ssc.Subscription)
                .WithMany(s => s.SubscriptionSubCourses)
                .HasForeignKey(ssc => ssc.sub_id)
                .HasPrincipalKey(s => s.sub_id)
                .OnDelete(DeleteBehavior.NoAction);


            // -----------------------------------------
            // SubscriptionSubCourse -> SubCourse
            // SubscriptionSubCourse.sid -> SubCourse.sid
            // -----------------------------------------

            modelBuilder.Entity<SubscriptionSubCourse>()
                .HasOne(ssc => ssc.SubCourse)
                .WithMany()
                .HasForeignKey(ssc => ssc.sid)
                .HasPrincipalKey(sc => sc.sid)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}