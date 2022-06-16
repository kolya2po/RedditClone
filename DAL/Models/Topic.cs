using System;
using System.Collections.Generic;

namespace Data.Models
{
    /// <summary>
    /// Represents topic (post).
    /// </summary>
    public class Topic : BaseModel
    {
        /// <summary>
        /// Topic's title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Topic's text (optional).
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Indicates if topic is pinned by moderator.
        /// </summary>
        public bool IsPinned { get; set; }

        /// <summary>
        /// Indicates if topic's comments are blocked by moderator.
        /// </summary>
        public bool CommentsAreBlocked { get; set; }

        /// <summary>
        /// Collection of topic's comments.
        /// </summary>
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();

        /// <summary>
        /// Topic's rating.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Topic's posting date.
        /// </summary>
        public DateTime PostingDate { get; set; }

        /// <summary>
        /// Topic's author's id.
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Navigation property, refers to user that has created topic.
        /// </summary>
        public User Author { get; set; }

        /// <summary>
        /// Topic's community's id.
        /// </summary>
        public int CommunityId { get; set; }


        /// <summary>
        /// Navigation property, refers to community in which topic is.
        /// </summary>
        public Community Community { get; set; }
    }
}
