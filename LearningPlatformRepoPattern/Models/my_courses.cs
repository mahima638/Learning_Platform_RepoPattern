using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatformRepoPattern.Models
{
    [Table("my_courses")]
    public class my_courses
    {
        [Key]
        public int mcid { get; set; }

        [Column("sid")]
        public int Sid { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }
        // Navigation property

       
        public SubCourse SubCourse { get; set; }
    }
}
