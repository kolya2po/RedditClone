using System.Collections.Generic;
using System.Threading.Tasks;
using Business.MapModels;

namespace Business.Interfaces
{
    public interface ICommentService
    {
        Task AddAsync(CommentModel model);

        Task UpdateAsync(CommentModel model);

        Task DeleteAsync(int modelId);

        Task ReplyToCommentAsync(int commentId, CommentModel model);
        Task IncreaseRatingAsync(int commentId);
        Task DecreaseRatingAsync(int commentId);

        Task<IEnumerable<CommentModel>> GetAllFromTopicAsync(int topicId);
        Task<IEnumerable<CommentModel>> GetAllFromUserAsync(int userId);
    }
}
