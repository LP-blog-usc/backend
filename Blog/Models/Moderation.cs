namespace Blog.Models
{
    public class Moderation
    {
        public int Id { get; set; }
        public bool IsApproved { get; set; }
        public string ModerationReason { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }

        public int ModeratorId { get; set; }
        public User Moderator { get; set; }
    }
}
