using System.Threading.Tasks;
using Data.Interfaces;
using Data.Repositories;

namespace Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ForumDbContext _context;
        private CommentRepository _commentRepository;
        private CommunityRepository _communityRepository;
        private RuleRepository _ruleRepository;
        private TopicRepository _topicRepository;
        private UserCommunityRepository _userCommunityRepository;
        public UnitOfWork(ForumDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public ICommentRepository CommentRepository => _commentRepository ??= new CommentRepository(_context);

        /// <inheritdoc />
        public ICommunityRepository CommunityRepository => _communityRepository ??= new CommunityRepository(_context);

        /// <inheritdoc />
        public IRuleRepository RuleRepository => _ruleRepository ??= new RuleRepository(_context);

        /// <inheritdoc />
        public ITopicRepository TopicRepository => _topicRepository ??= new TopicRepository(_context);

        /// <inheritdoc />
        public IUserCommunityRepository UserCommunityRepository =>
            _userCommunityRepository ??= new UserCommunityRepository(_context);

        /// <inheritdoc />
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}