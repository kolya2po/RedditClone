using System.Threading.Tasks;

namespace Data.Interfaces
{
    /// <summary>
    /// Contains all repositories. Also saves changes in database.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <inheritdoc cref="ICommentRepository"/>
        public ICommentRepository CommentRepository { get; }

        /// <inheritdoc cref="ICommunityRepository"/>
        public ICommunityRepository CommunityRepository { get; }

        /// <inheritdoc cref="IRuleRepository"/>
        public IRuleRepository RuleRepository { get; }

        /// <inheritdoc cref="ITopicRepository"/>
        public ITopicRepository TopicRepository { get; }

        /// <inheritdoc cref="IUserCommunityRepository"/>
        public IUserCommunityRepository UserCommunityRepository { get; }

        /// <summary>
        /// Saves changes in the database.
        /// </summary>
        /// <returns>Task.</returns>
        Task SaveAsync();
    }
}
