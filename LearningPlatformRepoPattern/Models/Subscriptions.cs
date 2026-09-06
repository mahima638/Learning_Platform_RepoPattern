using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatformRepoPattern.Models
{
    public class Subscriptions
    {
        [Key]
        public int sub_id { get; set; }

        [StringLength(100)]
        public string? sub_type { get; set; }

        public int mid { get; set; }

        public double sub_amount { get; set; }

        public string? subStatus { get; set; }

        public string? subThumbnail { get; set; }

        public MasterCourse? MasterCourse { get; set; }

        public ICollection<SubscriptionSubCourse> SubscriptionSubCourses { get; set; }
            = new List<SubscriptionSubCourse>();
    }
}