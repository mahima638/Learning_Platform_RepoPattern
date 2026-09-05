using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatformRepoPattern.Models
{
    [Table("TopicProgress")]
    public class TopicProgress
    {
        [Key]
        [Column("ProgressId")]
        public int ProgressId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("tid")]
        public int Tid { get; set; }

        [Column("sid")]
        public int Sid { get; set; }

        [Column("mcq_passed")]
        public bool McqPassed { get; set; }

        [Column("completed_at")]
        public DateTime? CompletedAt { get; set; }
    }
}
