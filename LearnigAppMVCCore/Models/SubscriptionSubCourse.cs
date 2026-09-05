using System.ComponentModel.DataAnnotations;

namespace LearnigAppMVCCore.Models
{
    public class SubscriptionSubCourse
    {
        [Key]
        public int id { get; set; }

        public int sub_id { get; set; }

        public int sid { get; set; }

        public virtual Subscriptions Subscription { get; set; }

        public virtual SubCourse SubCourse { get; set; }
    }
}
