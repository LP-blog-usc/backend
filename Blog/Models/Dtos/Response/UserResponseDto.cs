using System.ComponentModel.DataAnnotations;

namespace Blog.Models.Dtos.Response
{
    public class UserResponseDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string? TelephoneNumber { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
    }
}
