using System.Collections.Generic;

namespace Business.MapModels
{
    public class CommunityModel : BaseModel
    {
        public string Title { get; set; }
        public string About { get; set; }
        public int CreatorId { get; set; }
        public string CreationDate { get; set; }
        public int MembersCount { get; set; }
        public IEnumerable<TopicModel> PostModels { get; set; }
        public IEnumerable<RuleModel> RuleModels { get; set; }
        public IEnumerable<UserModel> ModeratorModels { get; set; }
    }
}