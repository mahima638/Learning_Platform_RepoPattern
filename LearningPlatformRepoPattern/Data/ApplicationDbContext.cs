using Microsoft.EntityFrameworkCore;
using LearningPlatformRepoPattern.Models;

namespace LearningPlatformRepoPattern.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Main tables
        public DbSet<MasterCourse> MasterCourses { get; set; }
        public DbSet<SubCourse> SubCourses { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Mcq> Mcqs { get; set; }

        // User tables
        public DbSet<User> Users { get; set; }
        public DbSet<my_courses> MyCourses { get; set; }
        public DbSet<TopicProgress> TopicProgress { get; set; }

        // Subscription tables
        public DbSet<Subscriptions> Subscriptions { get; set; }
        public DbSet<SubscriptionSubCourse> SubscriptionSubCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // MyCourses -> SubCourse
            modelBuilder.Entity<my_courses>()
                .HasOne(x => x.SubCourse)
                .WithMany()
                .HasForeignKey(x => x.Sid)
                .HasPrincipalKey(x => x.Id);

            // SubCourse -> MasterCourse
            modelBuilder.Entity<SubCourse>()
                .HasOne(x => x.MasterCourse)
                .WithMany(x => x.SubCourses)
                .HasForeignKey(x => x.MasterCourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Topic -> MasterCourse
            modelBuilder.Entity<Topic>()
                .HasOne(x => x.MasterCourse)
                .WithMany()
                .HasForeignKey(x => x.MasterCourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Topic -> SubCourse
            modelBuilder.Entity<Topic>()
                .HasOne(x => x.SubCourse)
                .WithMany()
                .HasForeignKey(x => x.SubCourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Material -> MasterCourse
            modelBuilder.Entity<Material>()
                .HasOne(x => x.MasterCourse)
                .WithMany()
                .HasForeignKey(x => x.MasterCourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Material -> SubCourse
            modelBuilder.Entity<Material>()
                .HasOne(x => x.SubCourse)
                .WithMany()
                .HasForeignKey(x => x.SubCourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Material -> Topic
            modelBuilder.Entity<Material>()
                .HasOne(x => x.Topic)
                .WithMany(x => x.Materials)
                .HasForeignKey(x => x.TopicId)
                .OnDelete(DeleteBehavior.Restrict);

            // MyCourses -> User
            modelBuilder.Entity<my_courses>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Subscriptions -> MasterCourse
            modelBuilder.Entity<Subscriptions>()
                .HasOne(x => x.MasterCourse)
                .WithMany()
                .HasForeignKey(x => x.mid)
                .OnDelete(DeleteBehavior.NoAction);

            // SubscriptionSubCourse -> Subscription
            modelBuilder.Entity<SubscriptionSubCourse>()
                .HasOne(x => x.Subscription)
                .WithMany(x => x.SubscriptionSubCourses)
                .HasForeignKey(x => x.sub_id)
                .OnDelete(DeleteBehavior.NoAction);

            // SubscriptionSubCourse -> SubCourse
            modelBuilder.Entity<SubscriptionSubCourse>()
                .HasOne(x => x.SubCourse)
                .WithMany()
                .HasForeignKey(x => x.sid)
                .OnDelete(DeleteBehavior.NoAction);

            // Decimal precision
            modelBuilder.Entity<SubCourse>()
                .Property(x => x.Amount)
                .HasPrecision(9, 2);

            // Mcq -> Material
            modelBuilder.Entity<Mcq>()
                .HasOne(x => x.Material)
                .WithMany(x => x.Mcqs)
                .HasForeignKey(x => x.MaterialId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}