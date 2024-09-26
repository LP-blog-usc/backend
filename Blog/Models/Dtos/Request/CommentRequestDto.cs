using System.ComponentModel.DataAnnotations;

namespace Blog.Models.Dtos.Request
{
    public class CommentRequestDto
    {
        [Required(ErrorMessage = "El contenido es obligatorio.")]
        [StringLength(500, ErrorMessage = "El contenido no puede tener más de 500 caracteres.")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Post Id es obligatorio.")]
        public int PostId { get; set; }

        [Required(ErrorMessage = "User Id es obligatorio.")]
        public int UserId { get; set; }
    }

}
