using System.ComponentModel.DataAnnotations;

namespace Blog.Models.Dtos.Request
{
    public class PostRequestDto
    {
        [Required(ErrorMessage = "El Autor es obligatorio.")]
        public int AuthorId { get; set; }
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(100, ErrorMessage = "El título no puede tener más de 100 caracteres.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "El cuerpo del post es obligatorio.")]
        [MinLength(50, ErrorMessage = "El cuerpo del post debe tener al menos 50 caracteres.")]
        [StringLength(500, ErrorMessage = "El cuerpo del post no puede exceder los 500 caracteres.")]
        public string Body { get; set; } = string.Empty;
    }
}
