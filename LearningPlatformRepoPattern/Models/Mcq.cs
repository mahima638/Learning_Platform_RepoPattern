using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatformRepoPattern.Models
{
    [Table("Mcq")]
    public class Mcq
    {
        [Key]
        [Column("mcq_id")]
        public int Id { get; set; }

        [Column("material_id")]
        public int MaterialId { get; set; }

        [Column("question")]
        public string Question { get; set; }

        [Column("option1")]
        public string Option1 { get; set; }

        [Column("option2")]
        public string Option2 { get; set; }

        [Column("option3")]
        public string Option3 { get; set; }

        [Column("option4")]
        public string Option4 { get; set; }

        [Column("answer")]
        public string Answer { get; set; }

        [ForeignKey("MaterialId")]
        public Material Material { get; set; }
    }
}