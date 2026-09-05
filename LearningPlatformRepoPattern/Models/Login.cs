using System.ComponentModel.DataAnnotations;

namespace LearningPlatformRepoPattern.Models
{
    public class Login
    {
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
