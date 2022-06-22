using System;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.MapModels;
using Business.Validation;
using Data.Interfaces;
using Data.Models;

namespace Business.Services
{
    /// <summary>
    /// Represents service that works with comments.
    /// </summary>
    public class CommentService : BaseService, ICommentService
    {
        /// <inheritdoc />
        public CommentService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        /// <inheritdoc />
        public async Task AddAsync(CommentModel model)
        {
            model.PostingDate = $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}";
            await UnitOfWork.CommentRepository.AddAsync(Mapper.Map<Comment>(model));
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        public async Task UpdateAsync(CommentModel model)
        {
            UnitOfWork.CommentRepository.Update(Mapper.Map<Comment>(model));
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(int modelId)
        {
            await UnitOfWork.CommentRepository.DeleteByIdAsync(modelId);
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        /// <exception cref="ForumException">Throws if comment to be replied doesn't exist.</exception>
        public async Task ReplyToCommentAsync(int commentId, CommentModel model)
        {
            var comment = await UnitOfWork.CommentRepository.GetByIdWithDetailsAsync(commentId);

            if (comment == null)
            {
                throw new ForumException("Comment doesn't exist.");
            }

            model.PostingDate = $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}";
            model.Text = $"{comment.Author.UserName}, {model.Text}";
            await UnitOfWork.CommentRepository.AddAsync(Mapper.Map<Comment>(model));
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        /// <exception cref="ForumException">Throws if comment which rating to be increased doesn't exist.</exception>
        public async Task IncreaseRatingAsync(int commentId)
        {
            var comment = await UnitOfWork.CommentRepository.GetByIdAsync(commentId);

            if (comment == null)
            {
                throw new ForumException("Comment doesn't exist.");
            }

            comment.Rating++;
            UnitOfWork.CommentRepository.Update(comment);
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        /// <exception cref="ForumException">Throws if comment which rating to be decreased doesn't exist.</exception>
        public async Task DecreaseRatingAsync(int commentId)
        {
            var comment = await UnitOfWork.CommentRepository.GetByIdAsync(commentId);

            if (comment == null)
            {
                throw new ForumException("Comment doesn't exist.");
            }
            comment.Rating--;
            UnitOfWork.CommentRepository.Update(comment);
            await UnitOfWork.SaveAsync();
        }
    }
}
