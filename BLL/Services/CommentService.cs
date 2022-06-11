﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.MapModels;
using Business.Validation;
using Data.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Business.Services
{
    public class CommentService : BaseService, ICommentService
    {
        private readonly UserManager<User> _userManager;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager) : base(unitOfWork, mapper)
        {
            _userManager = userManager;
        }

        /// <inheritdoc />
        public async Task AddAsync(CommentModel model)
        {
            ValidateCommentModel(model);

            model.PostingDate = DateTime.Now;
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

            model.PostingDate = DateTime.Now;
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

            comment.Rating++;
            UnitOfWork.CommentRepository.Update(comment);
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CommentModel>> GetAllFromTopicAsync(int topicId)
        {
            var topic = await UnitOfWork.TopicRepository.GetByIdWithDetailsAsync(topicId);

            if (topic == null)
            {
                throw new ForumException("Topic wasn't found.");
            }

            return Mapper.Map<IEnumerable<CommentModel>>(topic.Comments);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CommentModel>> GetAllFromUserAsync(int userId)
        {
            // TODO: Check if work.
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new ForumException("User wasn't found.");
            }

            return Mapper.Map<IEnumerable<CommentModel>>(user.Comments);
        }

        private static void ValidateCommentModel(CommentModel model)
        {
            if (model == null)
            {
                throw new ForumException("Passed model was equal null");
            }
            if (string.IsNullOrEmpty(model.Text))
            {
                throw new ForumException($"{nameof(model.Text)} was equal null");
            }
        }
    }
}