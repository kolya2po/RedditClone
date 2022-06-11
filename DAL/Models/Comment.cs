namespace Data.Models
{
    public class Comment : BaseModel
    {
        public string Text { get; set; }
        public int Rating { get; set; }
        public int AuthorId { get; set; }
        public User Author { get; set; }

        public int TopicId { get; set; }
        public Topic Topic { get; set; }
    }
}
