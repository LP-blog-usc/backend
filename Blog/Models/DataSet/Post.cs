using System.ComponentModel.DataAnnotations;

namespace Blog.Models.DataSet
{
    public class Post
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(100, ErrorMessage = "El título no puede tener más de 100 caracteres.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "El cuerpo del post es obligatorio.")]
        [MinLength(50, ErrorMessage = "El cuerpo del post debe tener al menos 50 caracteres.")]
        [StringLength(500, ErrorMessage = "El cuerpo del post no puede exceder los 500 caracteres.")]
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        [Required(ErrorMessage = "El autor es obligatorio.")]
        public int AuthorId { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Like>? Likes { get; set; }
        public Moderation? Moderation { get; set; }
    }
}
