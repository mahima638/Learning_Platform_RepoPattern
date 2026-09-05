using Microsoft.EntityFrameworkCore;
using LearningPlatformRepoPattern.Models;

namespace LearningPlatformRepoPattern.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<MasterCourse> MasterCourses { get; set; }
        public DbSet<SubCourse> SubCourses { get; set; }

        public DbSet<Topic> Topics { get; set; } 
        public DbSet<Material> Materials { get; set; }
        public DbSet<Mcq> Mcqs { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<my_courses> MyCourses { get; set; }
        public DbSet<TopicProgress> TopicProgress { get; set; }
    }
}
