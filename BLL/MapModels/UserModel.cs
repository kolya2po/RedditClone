using System;
using System.Collections.Generic;

namespace Business.MapModels
{
    public class UserModel : BaseModel
    {
        public string UserName { get; set; }
        public DateTime BirthDate { get; set; }
        public int Karma { get; set; }
        public int? ModeratedCommunityId { get; set; }
        public IEnumerable<int> PostsIds { get; set; }
        public IEnumerable<int> CommentsIds { get; set; }
    }
}
