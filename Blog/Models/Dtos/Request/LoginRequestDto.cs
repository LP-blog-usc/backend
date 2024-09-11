using System.ComponentModel.DataAnnotations;

namespace Blog.Models.Dtos.Request
{
    public class LoginRequestDto
    {

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; } = string.Empty;
    }
}
