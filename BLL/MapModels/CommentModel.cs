namespace Business.MapModels
{
    /// <summary>
    /// Represents Comment mapped model.
    /// </summary>
    public class CommentModel : BaseModel
    {
        /// <summary>
        /// Comment's text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Comment's rating.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Comment's author's id.
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Comment's author's name.
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// Comment's posting date.
        /// </summary>
        public string PostingDate { get; set; }

        /// <summary>
        /// Comment's topic's id..
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Comment's topic's name.
        /// </summary>
        public string TopicName { get; set; }

        /// <summary>
        /// Comment's community's name.
        /// </summary>
        public string CommunityName { get; set; }
    }
}
