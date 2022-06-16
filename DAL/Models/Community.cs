using System;
using System.Collections.Generic;

namespace Data.Models
{

    /// <summary>
    /// Represents community.
    /// </summary>
    public class Community : BaseModel
    {
        /// <summary>
        /// Title of community.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Info about community.
        /// </summary>
        public string About { get; set; }

        /// <summary>
        /// Id of the community's creator.
        /// </summary>
        public int CreatorId { get; set; }

        /// <summary>
        /// Navigation property refers to user that has created community.
        /// </summary>
        public User Creator { get; set; }

        /// <summary>
        /// Collection of community's members.
        /// </summary>

        public IEnumerable<UserCommunity> Members { get; set; } = new List<UserCommunity>();

        /// <summary>
        /// Collection of community's posts.
        /// </summary>
        public IEnumerable<Topic> Posts { get; set; } = new List<Topic>();

        /// <summary>
        /// Community's creation date.
        /// </summary>
        public DateTime CreationDate { get; set; }


        /// <summary>
        /// Collection of community's rules.
        /// </summary>
        public IEnumerable<Rule> Rules { get; set; } = new List<Rule>();

        /// <summary>
        /// Collection of community's moderators.
        /// </summary>
        public IEnumerable<User> Moderators { get; set; } = new List<User>();
    }
}
