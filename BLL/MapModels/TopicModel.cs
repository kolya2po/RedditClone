using System.Collections.Generic;

namespace Business.MapModels
{
    public class TopicModel : BaseModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public bool IsPinned { get; set; }
        public bool CommentsAreBlocked { get; set; }
        public string PostingDate { get; set; }
        public int CommentsCount { get; set; }
        public IEnumerable<CommentModel> CommentModels { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }

        public int CommunityId { get; set; }
        public string CommunityName { get; set; }
    }
}
