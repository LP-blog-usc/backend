using System.ComponentModel.DataAnnotations;

namespace Blog.Models.Dtos.UpdateDtos
{
    public class UserUpdateDto
    {
        [StringLength(50)]
        public string? Name { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [Phone]
        public string? TelephoneNumber { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string? Password { get; set; }

        public int RoleId { get; set; }
    }
}
