using System.Threading.Tasks;
using Business.MapModels;

namespace Business.Interfaces
{
    /// <summary>
    /// Describes CUD and forum-related operations with CommentModel entities.
    /// </summary>
    public interface ICommentService
    {
        /// <inheritdoc cref="ICrud{TModel}.AddAsync"/>
        Task AddAsync(CommentModel model);

        /// <inheritdoc cref="ICrud{TModel}.UpdateAsync"/>
        Task UpdateAsync(CommentModel model);

        /// <inheritdoc cref="ICrud{TModel}.DeleteAsync"/>
        Task DeleteAsync(int modelId);

        /// <summary>
        /// Creates asynchronously reply to the comment.
        /// </summary>
        /// <param name="commentId">Id of the comment to be replied.</param>
        /// <param name="model">Model of reply.</param>
        /// <returns>Task.</returns>
        Task ReplyToCommentAsync(int commentId, CommentModel model);

        /// <summary>
        /// Increases asynchronously comment's rating.
        /// </summary>
        /// <param name="commentId">Id of the comment which rating has to be increased.</param>
        /// <returns>Task.</returns>
        Task IncreaseRatingAsync(int commentId);

        /// <summary>
        /// Decreases asynchronously comment's rating.
        /// </summary>
        /// <param name="commentId">Id of the comment which rating has to be decreased.</param>
        /// <returns>Task.</returns>
        Task DecreaseRatingAsync(int commentId);
    }
}
