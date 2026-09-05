using System.ComponentModel.DataAnnotations;

namespace LearnigAppMVCCore.Models
{
    public class SubCourse
    {
        [Key]
        public int sid { get; set; }
        public int mid { get; set; }
        public string sname { get; set; }
        public string sstatus { get; set; }
    }
}
