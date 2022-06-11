using System;
using System.Collections.Generic;
using System.Linq;
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
    public class TopicService : BaseService, ITopicService
    {
        private readonly UserManager<User> _userManager;
        /// <inheritdoc />
        public TopicService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager) : base(unitOfWork, mapper)
        {
            _userManager = userManager;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TopicModel>> GetAllAsync()
        {
            var topics = await UnitOfWork.TopicRepository.GetAllWithDetailsAsync();

            return Mapper.Map<IEnumerable<TopicModel>>(topics);
        }

        /// <inheritdoc />
        public async Task<TopicModel> GetByIdAsync(int id)
        {
            var topic = await UnitOfWork.TopicRepository.GetByIdWithDetailsAsync(id);

            return Mapper.Map<TopicModel>(topic);
        }

        /// <inheritdoc />
        public async Task AddAsync(TopicModel model)
        {
            if (model == null)
            {
                throw new ForumException("Passed model was equal null.");
            }
            if (string.IsNullOrEmpty(model.Title))
            {
                throw new ForumException("Title cannot be null.");
            }

            model.PostingDate = DateTime.Now;
            await UnitOfWork.TopicRepository.AddAsync(Mapper.Map<Topic>(model));
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        public async Task UpdateAsync(TopicModel model)
        {
            if (model == null)
            {
                throw new ForumException("Passed model was equal null.");
            }
            if (string.IsNullOrEmpty(model.Title))
            {
                throw new ForumException("Title cannot be null.");
            }

            UnitOfWork.TopicRepository.Update(Mapper.Map<Topic>(model));
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(int modelId)
        {
            await UnitOfWork.TopicRepository.DeleteByIdAsync(modelId);
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        public async Task IncreaseRatingAsync(int topicId)
        {
            var topic = await UnitOfWork.TopicRepository.GetByIdAsync(topicId);

            if (topic == null)
            {
                throw new ForumException("Topic doesn't exist.");
            }

            topic.Rating++;
            UnitOfWork.TopicRepository.Update(topic);
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        public async Task DecreaseRatingAsync(int topicId)
        {
            var topic = await UnitOfWork.TopicRepository.GetByIdAsync(topicId);

            if (topic == null)
            {
                throw new ForumException("Topic doesn't exist.");
            }

            topic.Rating--;
            UnitOfWork.TopicRepository.Update(topic);
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        public async Task PinTopicAsync(int topicId)
        {
            var topic = await UnitOfWork.TopicRepository.GetByIdAsync(topicId);

            if (topic == null)
            {
                throw new ForumException("Topic doesn't exist.");
            }

            topic.IsPinned = true;
            UnitOfWork.TopicRepository.Update(topic);
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        public async Task BlockCommentsAsync(int topicId)
        {
            var topic = await UnitOfWork.TopicRepository.GetByIdAsync(topicId);

            if (topic == null)
            {
                throw new ForumException("Topic doesn't exist.");
            }

            topic.CommentsAreBlocked = true;
            UnitOfWork.TopicRepository.Update(topic);
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TopicModel>> GetAllMadeByUserAsync(int userId)
        {
            // TODO: Check if works.
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new ForumException("User doesn't exist.");
            }

            return Mapper.Map<IEnumerable<TopicModel>>(user.Posts);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TopicModel>> GetByFilterAsync(FilterSearchModel model)
        {
            var topics = await UnitOfWork.TopicRepository.GetAllWithDetailsAsync();
            if (model.IsNew && !model.IsTop)
            {
                var searched = topics.OrderBy(c => c.PostingDate);
                return Mapper.Map<IEnumerable<TopicModel>>(searched);
            }

            if (model.IsTop && !model.IsNew)
            {
                var searched = topics.OrderByDescending(c => c.Rating);
                return Mapper.Map<IEnumerable<TopicModel>>(searched);
            }

            return Mapper.Map<IEnumerable<TopicModel>>(topics);
        }
    }
}
