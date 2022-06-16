namespace Business.MapModels
{
    public class CommentModel : BaseModel
    {
        public string Text { get; set; }
        public int Rating { get; set; }
        public int AuthorId { get; set; }
        public string PostingDate { get; set; }
        public string AuthorName { get; set; }
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public string CommunityName { get; set; }
    }
}
