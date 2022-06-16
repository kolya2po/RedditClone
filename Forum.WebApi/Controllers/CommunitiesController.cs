using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.MapModels;
using Forum.WebApi.Models.Community;
using Forum.WebApi.Models.Rule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommunitiesController : BaseController
    {
        private readonly ICommunityService _communityService;
        private readonly IRuleService _ruleService;
        public CommunitiesController(IMapper mapper, ICommunityService communityService, IRuleService ruleService) : base(mapper)
        {
            _communityService = communityService;
            _ruleService = ruleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _communityService.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _communityService.GetByIdAsync(id));
        }

        [HttpGet("{id:int}/users")]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> GetAllUsers(int id)
        {
            return Ok(await _communityService.GetAllUsers(id));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(CreateCommunityDto model)
        {
            return Ok(await _communityService.AddAsync(Mapper.Map<CommunityModel>(model)));
        }

        [HttpPut]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> Update(UpdateCommunityDto model)
        {
            await _communityService.UpdateAsync(Mapper.Map<CommunityModel>(model));
            return Ok();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Moderator")]
        public async Task Delete(int id)
        {
            await _communityService.DeleteAsync(id);
        }

        [HttpPost("add-moderator")]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> AddModerator(int userId, int communityId)
        {
            await _communityService.AddModeratorAsync(userId, communityId);
            return Ok();
        }

        [HttpDelete("remove-moderator")]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> RemoveModerator(int userId, int communityId)
        {
            await _communityService.RemoveModeratorAsync(userId, communityId);
            return Ok();
        }

        [HttpPost("join")]
        [Authorize]
        public async Task<IActionResult> Join(int userId, int communityId)
        {
            await _communityService.JoinCommunityAsync(userId, communityId);
            return Ok();
        }

        [HttpDelete("leave")]
        [Authorize]
        public async Task<IActionResult> Leave(int userId, int communityId)
        {
            await _communityService.LeaveCommunityAsync(userId, communityId);
            return Ok();
        }

        [HttpPost("{communityId:int}/add-rule")]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> AddRule(int communityId, CreateRuleDto model)
        {
            model.CommunityId = communityId;
            await _ruleService.AddAsync(Mapper.Map<RuleModel>(model));
            return Ok();
        }

        [HttpPut("update-rule")]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> UpdateRule(UpdateRuleDto model)
        {
            await _ruleService.UpdateAsync(Mapper.Map<RuleModel>(model));
            return Ok();
        }

        [HttpDelete("delete-rule/{ruleId:int}")]
        [Authorize(Roles = "Moderator")]
        public async Task DeleteRule(int ruleId)
        {
            await _ruleService.DeleteAsync(ruleId);
        }
    }
}
