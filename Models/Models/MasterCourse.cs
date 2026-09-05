using System.ComponentModel.DataAnnotations;

namespace LearnigAppMVCCore.Models
{
    public class MasterCourse
    {
        [Key]
        public int mid { get; set; }

        public string mname { get; set; }

        public string mstatus { get; set; }
    }
}
