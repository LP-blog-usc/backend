using System.ComponentModel.DataAnnotations;

namespace Blog.Models.Dtos.UpdateDtos
{
    public class CommentUpdateDto
    {
        [Required(ErrorMessage = "El contenido es obligatorio.")]
        [StringLength(500, ErrorMessage = "El contenido no puede tener más de 500 caracteres.")]
        public string Content { get; set; }
    }

}
