using System;

namespace Data.Models
{
    /// <summary>
    /// Represents comment.
    /// </summary>
    public class Comment : BaseModel
    {
        /// <summary>
        /// Text of comment.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Rating of comment.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Id of comment's author.
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Navigation property, refers to user that has created comment.
        /// </summary>
        public User Author { get; set; }

        /// <summary>
        /// Posting date of comment.
        /// </summary>
        public DateTime PostingDate { get; set; }

        /// <summary>
        /// Id of comment's topic.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Navigation property, refers to topic that contains comment.
        /// </summary>
        public Topic Topic { get; set; }
    }
}
