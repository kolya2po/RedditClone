using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.MapModels;
using Business.Validation;
using Data.Interfaces;
using Data.Models;

namespace Business.Services
{
    public class CommentService : BaseService, ICommentService
    {
        public CommentService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        /// <inheritdoc />
        public async Task AddAsync(CommentModel model)
        {
            ValidateCommentModel(model);

            model.PostingDate = $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}";
            await UnitOfWork.CommentRepository.AddAsync(Mapper.Map<Comment>(model));
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        public async Task UpdateAsync(CommentModel model)
        {
            ValidateCommentModel(model);
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
        private static void ValidateCommentModel(CommentModel model)
        {
            if (model == null)
            {
                throw new ForumException("Passed model was equal null");
            }
        }
    }
}
