﻿using System.ComponentModel.DataAnnotations;

namespace Blog.Models.DataSet
{
    public class Comment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El contenido es obligatorio.")]
        [StringLength(500, ErrorMessage = "El contenido no puede tener más de 500 caracteres.")]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Post Id es obligatorio.")]
        public int PostId { get; set; }
        public Post Post { get; set; }

        [Required(ErrorMessage = "User Id es obligatorio.")]
        public int UserId { get; set; }
        public User User { get; set; }
    }

}
