using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatformRepoPattern.Models
{
    [Table("Master_course")]
    public class MasterCourse
    {
        [Key]
        [Column("mid")]
        public int Id { get; set; }

        [Column("mname")]
        public string CourseName { get; set; }

        [Column("mstatus")]
        public string Status { get; set; }

        [Column("mthumbnail")]
        public string ThumbnailPath { get; set; }

        [Column("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("createdBy")]
        public string CreatedBy { get; set; }

        public ICollection<SubCourse> SubCourses { get; set; }
    }
}
