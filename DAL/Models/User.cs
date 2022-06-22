using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Data.Models
{
    /// <summary>
    /// Represents application's user. Extends IdentityUser<int> class,
    /// by adding new properties.
    /// </summary>
    public class User : IdentityUser<int>
    {
        /// <summary>
        /// User's birth date.
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// User's karma.
        /// </summary>
        public int Karma { get; set; }

        /// <summary>
        /// Id of community that has been created by user.
        /// </summary>

        public int? CreatedCommunityId { get; set; }

        /// <summary>
        /// Id of community that moderated by user.
        /// </summary>

        public int? ModeratedCommunityId { get; set; }

        /// <summary>
        /// Navigation property, refers to community that is moderated by user.
        /// </summary>
        public Community ModeratedCommunity { get; set; }

        /// <summary>
        /// Collection of communities in which the user is present.
        /// </summary>

        public IEnumerable<UserCommunity> Communities { get; set; } = new List<UserCommunity>();

        /// <summary>
        /// Collection of posts that have been made by user.
        /// </summary>
        public IEnumerable<Topic> Posts { get; set; } = new List<Topic>();

        /// <summary>
        /// Collection of comments that have been made by user.
        /// </summary>
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
    }
}
