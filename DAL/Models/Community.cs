using System;
using System.Collections.Generic;

namespace Data.Models
{
    public class Community : BaseModel
    {
        public string Title { get; set; }
        public string About { get; set; }

        public int CreatorId { get; set; }
        public User Creator { get; set; }

        public IEnumerable<UserCommunity> Members { get; set; } = new List<UserCommunity>();
        public IEnumerable<Topic> Posts { get; set; } = new List<Topic>();

        public DateTime CreationDate { get; set; }

        public IEnumerable<Rule> Rules { get; set; } = new List<Rule>();
        public IEnumerable<User> Moderators { get; set; } = new List<User>();
    }
}
