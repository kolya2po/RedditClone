using System;
using System.Collections.Generic;

namespace Data.Models
{
    public class Topic : BaseModel
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public bool IsPinned { get; set; }

        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
        public int Rating { get; set; }
        public DateTime PostingDate { get; set; }

        public int UserId { get; set; }
        public User Author { get; set; }

        public int CommunityId { get; set; }
        public Community Community { get; set; }
    }
}
