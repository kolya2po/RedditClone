using System.Threading.Tasks;
using Business.MapModels;

namespace Business.Interfaces
{
    public interface ICommentService
    {
        Task<CommentModel> AddAsync(CommentModel model);

        Task UpdateAsync(CommentModel model);

        Task DeleteAsync(int modelId);

        Task<CommentModel> ReplyToCommentAsync(int commentId, CommentModel model);
        Task IncreaseRatingAsync(int commentId);
        Task DecreaseRatingAsync(int commentId);
    }
}
