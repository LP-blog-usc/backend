using Blog.Models.DataSet;

namespace Blog.Models.InnerModels
{
    public class PostWithAuthor
    {
        public Post Post { get; set; }
        public string AuthorName { get; set; }
    }
}
