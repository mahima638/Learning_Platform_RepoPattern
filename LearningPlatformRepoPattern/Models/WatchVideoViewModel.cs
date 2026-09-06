namespace LearningPlatformRepoPattern.Models
{
    public class WatchVideoViewModel
    {
        public int Sid { get; set; }

        public int UserId { get; set; }

        public int CurrentTopicId { get; set; }

        public string SubCourseName { get; set; }

        public string CourseName { get; set; }

        public string Mthumbnail { get; set; }

        public List<TopicItemViewModel> TopicItems { get; set; }
            = new List<TopicItemViewModel>();

        public List<Mcq> Mcqs { get; set; }
            = new List<Mcq>();

        public bool CertificateUnlocked { get; set; }
    }
}