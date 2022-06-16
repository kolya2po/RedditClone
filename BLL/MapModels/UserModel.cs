using System.Collections.Generic;

namespace Business.MapModels
{
    public class UserModel : BaseModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string BirthDate { get; set; }
        public int Karma { get; set; }
        public int? ModeratedCommunityId { get; set; }
        public int? CreatedCommunityId { get; set; }
        public IEnumerable<TopicModel> PostModels { get; set; }
        public IEnumerable<CommentModel> CommentModels { get; set; }
    }
}
