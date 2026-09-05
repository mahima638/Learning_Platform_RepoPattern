using Microsoft.EntityFrameworkCore;
using LearningPlatformRepoPattern.Models;

namespace LearningPlatformRepoPattern.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<MasterCourse> MasterCourses { get; set; }
        public DbSet<SubCourse> SubCourses { get; set; }
    }
}
