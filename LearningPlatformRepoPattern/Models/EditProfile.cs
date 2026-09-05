using System.ComponentModel.DataAnnotations;

namespace LearningPlatformRepoPattern.Models
{
    public class EditProfile
    {
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; } 

        [Required]
        [EmailAddress]
        public string UserEmail { get; set; } 
    }
}
