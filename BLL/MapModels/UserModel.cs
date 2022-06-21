using System.Collections.Generic;

namespace Business.MapModels
{
    /// <summary>
    /// Represents User mapped model.
    /// </summary>
    public class UserModel : BaseModel
    {
        /// <summary>
        /// User's name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// User's email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's birth date.
        /// </summary>
        public string BirthDate { get; set; }

        /// <summary>
        /// User's karma.
        /// </summary>
        public int Karma { get; set; }

        /// <summary>
        /// User's moderated community's id.
        /// </summary>
        public int? ModeratedCommunityId { get; set; }

        /// <summary>
        /// User's created community's id.
        /// </summary>
        public int? CreatedCommunityId { get; set; }

        /// <summary>
        /// Collection of ids of communities in which user participates.
        /// </summary>
        public IEnumerable<int> CommunitiesIds { get; set; }

        /// <summary>
        /// Collection of mapped models of topics made by user.
        /// </summary>
        public IEnumerable<TopicModel> PostModels { get; set; }

        /// <summary>
        /// Collection of mapped models of comments made by user.
        /// </summary>
        public IEnumerable<CommentModel> CommentModels { get; set; }

        /// <summary>
        /// User's JWT token.
        /// </summary>
        public string Token { get; set; }
    }
}
