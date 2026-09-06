using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatformRepoPattern.Models
{
    [Table("Topic")]
    public class Topic
    {
        [Key]
        [Column("tid")]
        public int Id { get; set; }

        [Column("mid")]
        public int MasterCourseId { get; set; }

        [Column("sid")]
        public int SubCourseId { get; set; }

        [Column("tname")]
        public string TopicName { get; set; }

        [Column("videoUrl")]
        public string VideoUrl { get; set; }

        [Column("tstatus")]
        public string Status { get; set; }

        [Column("tthumbnail")]
        public string ThumbnailPath { get; set; }

        [ForeignKey("MasterCourseId")]
        public MasterCourse MasterCourse { get; set; }

        [ForeignKey("SubCourseId")]
        public SubCourse SubCourse { get; set; }

        public ICollection<Material> Materials { get; set; } = new List<Material>();
    }
}