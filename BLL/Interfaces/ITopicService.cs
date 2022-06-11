using System.Collections.Generic;
using System.Threading.Tasks;
using Business.MapModels;

namespace Business.Interfaces
{
    public interface ITopicService : ICrud<TopicModel>
    {
        Task IncreaseRatingAsync(int topicId);
        Task DecreaseRatingAsync(int topicId);
        Task PinTopicAsync(int topicId);
        Task BlockCommentsAsync(int topicId);
        Task<IEnumerable<TopicModel>> GetAllMadeByUserAsync(int userId);
        Task<IEnumerable<TopicModel>> GetByFilterAsync(FilterSearchModel model);
    }
}
