using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.MapModels;
using Forum.WebApi.Models.Topic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.WebApi.Controllers
{
    /// <summary>
    /// Represents controller that handles requests related to topics.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TopicsController : BaseController
    {
        private readonly ITopicService _topicService;

        /// <summary>
        /// Initializes new instance of the TopicsController class and class's fields.
        /// </summary>
        /// <param name="mapper">Instance of Mapper class.</param>
        /// <param name="topicService">Instance of TopicService class.</param>
        public TopicsController(IMapper mapper, ITopicService topicService) : base(mapper)
        {
            _topicService = topicService;
        }

        //  GET: HTTP://http://localhost:5001/api/topics
        /// <summary>
        /// Displays all topics.
        /// </summary>
        /// <returns>Topics.</returns>
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _topicService.GetAllAsync());
        }

        //  GET: HTTP://http://localhost:5001/api/topics/1
        /// <summary>
        /// Displays topic with specified id.
        /// </summary>
        /// <param name="id">Topic's id.</param>
        /// <returns>Topic model with specified id.</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _topicService.GetByIdAsync(id));
        }

        //  POST: HTTP://http://localhost:5001/api/topics
        /// <summary>
        /// Add new topic to the db table.
        /// </summary>
        /// <param name="model">DTO that encapsulates information needed
        /// for creating of the new topic.</param>
        /// <returns>Created topic model.</returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(CreateTopicDto model)
        {
            return Ok(await _topicService.AddAsync(Mapper.Map<TopicModel>(model)));
        }

        //  PUT: HTTP://http://localhost:5001/api/topics
        /// <summary>
        /// Updates topic in the db table.
        /// </summary>
        /// <param name="model">DTO that encapsulates information needed
        /// for updating of the topic.</param>
        /// <returns>Task.</returns>
        [HttpPut]
        [Authorize]
        public async Task Update(UpdateTopicDto model)
        {
            await _topicService.UpdateAsync(Mapper.Map<TopicModel>(model));
        }

        //  DELETE: HTTP://http://localhost:5001/api/topics/1
        /// <summary>
        /// Deletes topic from the db table.
        /// </summary>
        /// <param name="id">Id of the topic to be deleted.</param>
        /// <returns>Task.</returns>
        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task Delete(int id)
        {
            await _topicService.DeleteAsync(id);
        }

        //  PUT: HTTP://http://localhost:5001/api/topics/1/increase-rating
        /// <summary>
        /// Increases topic's rating.
        /// </summary>
        /// <param name="id">Id of the topic which topic's rating has to be increased.</param>
        /// <returns>Task.</returns>
        [HttpPut("{id:int}/increase-rating")]
        [Authorize]
        public async Task IncreaseRating(int id)
        {
            await _topicService.IncreaseRatingAsync(id);
        }

        //  PUT: HTTP://http://localhost:5001/api/topics/1/decrease-rating
        /// <summary>
        /// Decreases topic's rating.
        /// </summary>
        /// <param name="id">Id of the topic which topic's rating has to be decreased.</param>
        /// <returns></returns>
        [HttpPut("{id:int}/decrease-rating")]
        [Authorize]
        public async Task DecreaseRating(int id)
        {
            await _topicService.DecreaseRatingAsync(id);
        }

        //  PUT: HTTP://http://localhost:5001/api/topics/1/pin
        /// <summary>
        /// Pins topic.
        /// </summary>
        /// <param name="id">Id of the topic to be pinned.</param>
        /// <returns>Task.</returns>
        [HttpPut("{id:int}/pin")]
        [Authorize(Policy = "ModeratorOnly")]
        public async Task Pin(int id)
        {
            await _topicService.PinTopicAsync(id);
        }

        //  PUT: HTTP://http://localhost:5001/api/topics/1/unpin
        /// <summary>
        /// Unpins topic.
        /// </summary>
        /// <param name="id">Id of the topic to be unpinned.</param>
        /// <returns>Task.</returns>
        [HttpPut("{id:int}/unpin")]
        [Authorize(Policy = "ModeratorOnly")]
        public async Task Unpin(int id)
        {
            await _topicService.UnpinTopicAsync(id);
        }

        //  PUT: HTTP://http://localhost:5001/api/communities/topics/1/block-comments
        /// <summary>
        /// Blocks creation of the comments for the topic.
        /// </summary>
        /// <param name="id">Id of the topic which comments to be added..</param>
        /// <returns>Task.</returns>
        [HttpPut("{id:int}/block-comments")]
        [Authorize(Policy = "ModeratorOnly")]
        public async Task BlockComments(int id)
        {
            await _topicService.BlockCommentsAsync(id);
        }
    }
}
