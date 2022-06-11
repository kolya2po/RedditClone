using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUnitOfWork
    {
        public ICommentRepository CommentRepository { get; }
        public ICommunityRepository CommunityRepository { get; }
        public IRuleRepository RuleRepository { get; }
        public ITopicRepository TopicRepository { get; }
        public IUserCommunityRepository UserCommunityRepository { get; }
        Task SaveAsync();
    }
}
