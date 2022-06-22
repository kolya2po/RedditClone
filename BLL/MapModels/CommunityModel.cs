using System.Collections.Generic;

namespace Business.MapModels
{
    /// <summary>
    /// Represents Community mapped model.
    /// </summary>
    public class CommunityModel : BaseModel
    {
        /// <summary>
        /// Community's title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Information about community.
        /// </summary>
        public string About { get; set; }

        /// <summary>
        /// Community's creator's id.
        /// </summary>
        public int CreatorId { get; set; }

        /// <summary>
        /// Community's creation date.
        /// </summary>
        public string CreationDate { get; set; }

        /// <summary>
        /// Community's members count.
        /// </summary>
        public int MembersCount { get; set; }

        /// <summary>
        /// Collection of community's topic models.
        /// </summary>
        public IEnumerable<TopicModel> PostModels { get; set; }

        /// <summary>
        /// Collection of community's rule models.
        /// </summary>
        public IEnumerable<RuleModel> RuleModels { get; set; }

        /// <summary>
        /// Collection of community's moderator models.
        /// </summary>
        public IEnumerable<UserModel> ModeratorModels { get; set; }
    }
}