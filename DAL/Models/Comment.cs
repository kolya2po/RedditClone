using System.Collections.Generic;

namespace Data.Models
{
    public class Comment : BaseModel
    {
        public string Text { get; set; }
        public IEnumerable<Comment> Replies { get; set; } = new List<Comment>();
        public int Rating { get; set; }
        public int UserId { get; set; }
        public User Author { get; set; }

        public int TopicId { get; set; }
        public Topic Topic { get; set; }
    }
}
