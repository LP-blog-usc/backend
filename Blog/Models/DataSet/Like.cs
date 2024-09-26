using System.ComponentModel.DataAnnotations;

namespace Blog.Models.DataSet
{
    public class Like
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El PostId es obligatorio.")]
        public int PostId { get; set; }
        public Post Post { get; set; }

        [Required(ErrorMessage = "El UserId es obligatorio.")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
