using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatformRepoPattern.Models
{
    [Table("sub_course")]
    public class SubCourse
    {
        [Key]
        [Column("sid")]
        public int Id { get; set; }

        [Column("mid")]
        public int MasterCourseId { get; set; }

        [Column("sname")]
        public string SubCourseName { get; set; }

        [Column("sstatus")]
        public string Status { get; set; }

        [Column("samount", TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Column("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("createdBy")]
        public string CreatedBy { get; set; }

        [ForeignKey("MasterCourseId")]
        public MasterCourse MasterCourse { get; set; }
    }
}
