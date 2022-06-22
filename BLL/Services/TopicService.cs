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

namespace Business.Services
{
    /// <summary>
    /// Represents service that works with topics.
    /// </summary>
    public class TopicService : BaseService, ITopicService
    {
        /// <inheritdoc />
        public TopicService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TopicModel>> GetAllAsync()
        {
            var topics = await UnitOfWork.TopicRepository.GetAllWithDetailsAsync();
            var models = Mapper.Map<IEnumerable<TopicModel>>(topics).ToList();

            foreach (var m in models)
            {
                m.CommentModels = null;
            }

            return models;
        }

        /// <inheritdoc />
        public async Task<TopicModel> GetByIdAsync(int id)
        {
            var topic = await UnitOfWork.TopicRepository.GetByIdWithDetailsAsync(id);

            return Mapper.Map<TopicModel>(topic);
        }

        /// <inheritdoc />
        public async Task<TopicModel> AddAsync(TopicModel model)
        {
            model.PostingDate = $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}";
            await UnitOfWork.TopicRepository.AddAsync(Mapper.Map<Topic>(model));
            await UnitOfWork.SaveAsync();

            return model;
        }

        /// <inheritdoc />
        public async Task UpdateAsync(TopicModel model)
        {
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
        /// <exception cref="ForumException">Throws if topic doesn't exist.</exception>
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
        /// <exception cref="ForumException">Throws if topic doesn't exist.</exception>
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
        /// <exception cref="ForumException">Throws if topic doesn't exist.</exception>
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
        /// <exception cref="ForumException">Throws if topic doesn't exist.</exception>
        public async Task UnpinTopicAsync(int topicId)
        {
            var topic = await UnitOfWork.TopicRepository.GetByIdAsync(topicId);

            if (topic == null)
            {
                throw new ForumException("Topic doesn't exist.");
            }

            topic.IsPinned = false;
            UnitOfWork.TopicRepository.Update(topic);
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        /// <exception cref="ForumException">Throws if topic doesn't exist.</exception>
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
    }
}
