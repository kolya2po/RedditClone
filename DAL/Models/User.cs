using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Data.Models
{
    public class User : IdentityUser<int>
    {
        public DateTime BirthDate { get; set; }
        public int Karma { get; set; }

        public int? CreatedCommunityId { get; set; }
        public Community CreatedCommunity { get; set; }

        public int? ModeratedCommunityId { get; set; }
        public Community ModeratedCommunity { get; set; }

        public IEnumerable<UserCommunity> Communities { get; set; } = new List<UserCommunity>();

        public IEnumerable<Topic> Posts { get; set; } = new List<Topic>();
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
    }
}
