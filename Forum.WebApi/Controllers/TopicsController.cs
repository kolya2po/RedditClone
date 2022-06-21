using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.MapModels;
using Forum.WebApi.Models.Topic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TopicsController : BaseController
    {
        private readonly ITopicService _topicService;

        public TopicsController(IMapper mapper, ITopicService topicService) : base(mapper)
        {
            _topicService = topicService;
        }

        public async Task<IActionResult> GetAll()
        {
            return Ok(await _topicService.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _topicService.GetByIdAsync(id));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(CreateTopicDto model)
        {
            return Ok(await _topicService.AddAsync(Mapper.Map<TopicModel>(model)));
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(UpdateTopicDto model)
        {
            await _topicService.UpdateAsync(Mapper.Map<TopicModel>(model));
            return Ok();
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task Delete(int id)
        {
            await _topicService.DeleteAsync(id);
        }

        [HttpPut("{id:int}/increase-rating")]
        [Authorize]
        public async Task IncreaseRating(int id)
        {
            await _topicService.IncreaseRatingAsync(id);
        }

        [HttpPut("{id:int}/decrease-rating")]
        [Authorize]
        public async Task DecreaseRating(int id)
        {
            await _topicService.DecreaseRatingAsync(id);
        }

        [HttpPut("{id:int}/pin")]
        [Authorize(Policy = "ModeratorOnly")]
        public async Task Pin(int id)
        {
            await _topicService.PinTopicAsync(id);
        }

        [HttpPut("{id:int}/unpin")]
        [Authorize(Policy = "ModeratorOnly")]
        public async Task Unpin(int id)
        {
            await _topicService.UnpinTopicAsync(id);
        }

        [HttpPut("{id:int}/block-comments")]
        [Authorize(Policy = "ModeratorOnly")]
        public async Task BlockComments(int id)
        {
            await _topicService.BlockCommentsAsync(id);
        }
    }
}
