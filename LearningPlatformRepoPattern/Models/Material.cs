using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatformRepoPattern.Models
{
    [Table("Material")]
    public class Material
    {
        [Key]
        [Column("material_id")]
        public int Id { get; set; }

        [Column("mid")]
        public int MasterCourseId { get; set; }

        [Column("sid")]
        public int SubCourseId { get; set; }

        [Column("tid")]
        public int TopicId { get; set; }

        [Column("assignment")]
        public string Assignment { get; set; }

        [ForeignKey("MasterCourseId")]
        public MasterCourse MasterCourse { get; set; }

        [ForeignKey("SubCourseId")]
        public SubCourse SubCourse { get; set; }

        [ForeignKey("TopicId")]
        public Topic Topic { get; set; }

        public ICollection<Mcq> Mcqs { get; set; }
    }
}