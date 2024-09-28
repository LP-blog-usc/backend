using System.ComponentModel.DataAnnotations;

namespace Blog.Models.Dtos.Request
{
    public class LikeRequestDto
    {
        [Required(ErrorMessage = "El PostId es obligatorio.")]
        public int PostId { get; set; }

        [Required(ErrorMessage = "El UserId es obligatorio.")]
        public int UserId { get; set; }
    }

}
