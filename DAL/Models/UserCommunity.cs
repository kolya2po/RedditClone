namespace Data.Models
{
    /// <summary>
    /// Represents relation between user and community.
    /// </summary>
    public class UserCommunity : BaseModel
    {
        /// <summary>
        /// User's id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Navigation property that refers to user.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Community's id.
        /// </summary>
        public int CommunityId { get; set; }

        /// <summary>
        /// Navigation property that refers to community.
        /// </summary>
        public Community Community { get; set; }
    }
}
