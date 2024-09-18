using System.ComponentModel.DataAnnotations;

namespace Blog.Models.Dtos.UpdateDtos
{
    public class PostUpdateDto
    {
        [Required(ErrorMessage = "El Autor es obligatorio.")]
        public int AuthorId { get; set; }
        
        [StringLength(100, ErrorMessage = "El título no puede tener más de 100 caracteres.")]
        public string Title { get; set; } = string.Empty;

        [MinLength(50, ErrorMessage = "El cuerpo del post debe tener al menos 50 caracteres.")]
        [StringLength(500, ErrorMessage = "El cuerpo del post no puede exceder los 500 caracteres.")]
        public string Body { get; set; } = string.Empty;
    }
}
