using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatformRepoPattern.Models
{
    [Table("my_courses")]
    public class my_courses
    {
        [Column("sid")]
        public int Sid { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }
    }
}
