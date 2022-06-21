using System.Collections.Generic;

namespace Business.MapModels
{
    /// <summary>
    /// Represents Topic mapped model.
    /// </summary>
    public class TopicModel : BaseModel
    {
        /// <summary>
        /// Topic's title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Topic's title.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Topic's title.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Topic's title.
        /// </summary>
        public bool IsPinned { get; set; }

        /// <summary>
        /// Topic's title.
        /// </summary>
        public bool CommentsAreBlocked { get; set; }

        /// <summary>
        /// Topic's title.
        /// </summary>
        public string PostingDate { get; set; }

        /// <summary>
        /// Topic's title.
        /// </summary>
        public int CommentsCount { get; set; }

        /// <summary>
        /// Topic's title.
        /// </summary>
        public IEnumerable<CommentModel> CommentModels { get; set; }

        /// <summary>
        /// Topic's title.
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Topic's title.
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// Topic's title.
        /// </summary>

        public int CommunityId { get; set; }

        /// <summary>
        /// Topic's title.
        /// </summary>
        public string CommunityName { get; set; }
    }
}
