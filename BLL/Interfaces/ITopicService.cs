using System.Threading.Tasks;
using Business.MapModels;

namespace Business.Interfaces
{
    /// <summary>
    /// Describes CRUD and forum-related operations with models.
    /// </summary>
    public interface ITopicService : ICrud<TopicModel>
    {
        /// <summary>
        /// Increases asynchronously comment's rating.
        /// </summary>
        /// <param name="topicId">Id of the comment which rating has to be increased.</param>
        /// <returns>Task.</returns>
        Task IncreaseRatingAsync(int topicId);

        /// <summary>
        /// Decreases asynchronously topic's rating.
        /// </summary>
        /// <param name="topicId">Id of the topic which rating has to be decreased.</param>
        /// <returns>Task.</returns>
        Task DecreaseRatingAsync(int topicId);

        /// <summary>
        /// Sets asynchronously IsPinned property of the topic entity to true.
        /// </summary>
        /// <param name="topicId">Id of the topic which property has to be changed.</param>
        /// <returns>Task.</returns>
        Task PinTopicAsync(int topicId);

        /// <summary>
        /// Sets asynchronously IsPinned property of the topic entity to false.
        /// </summary>
        /// <param name="topicId">Id of the topic which property has to be changed.</param>
        /// <returns>Task.</returns>
        Task UnpinTopicAsync(int topicId);

        /// <summary>
        /// Sets asynchronously CommentsAreBlocked property of the topic entity to false.
        /// </summary>
        /// <param name="topicId">Id of the topic which property has to be changed.</param>
        /// <returns>Task.</returns>
        Task BlockCommentsAsync(int topicId);
    }
}
