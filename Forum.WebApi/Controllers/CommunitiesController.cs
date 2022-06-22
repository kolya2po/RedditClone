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
    /// <summary>
    /// Represents controller that handles requests related to communities.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CommunitiesController : BaseController
    {
        private readonly ICommunityService _communityService;
        private readonly IRuleService _ruleService;

        /// <summary>
        /// Initializes new instance of the CommunitiesController class and class's fields.
        /// </summary>
        /// <param name="mapper">>Instance of Mapper class.</param>
        /// <param name="communityService">>Instance of CommunityService class.</param>
        /// <param name="ruleService">>Instance of RuleService class.</param>
        public CommunitiesController(IMapper mapper, ICommunityService communityService, IRuleService ruleService) : base(mapper)
        {
            _communityService = communityService;
            _ruleService = ruleService;
        }

        //  GET: HTTP://http://localhost:5001/api/communities
        /// <summary>
        /// Displays all community models.
        /// </summary>
        /// <returns>All communities.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _communityService.GetAllAsync());
        }

        //  GET: HTTP://http://localhost:5001/api/communities/1
        /// <summary>
        /// Displays community model with specified id.
        /// </summary>
        /// <param name="id">Community's id.</param>
        /// <returns>Community model with specified id.</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _communityService.GetByIdAsync(id));
        }

        //  GET: HTTP://http://localhost:5001/api/communities/1/users
        /// <summary>
        /// Displays all users those are in the community.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Collection of users.</returns>
        [HttpGet("{id:int}/users")]
        [Authorize(Policy = "ModeratorOnly")]
        public async Task<IActionResult> GetAllUsers(int id)
        {
            return Ok(await _communityService.GetAllUsersAsync(id));
        }

        //  POST: HTTP://http://localhost:5001/api/communities
        /// <summary>
        /// Adds new community to the db table.
        /// </summary>
        /// <param name="model">DTO that encapsulates information needed
        /// for creation of the new community.</param>
        /// <returns>Created community.</returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(CreateCommunityDto model)
        {
            return Ok(await _communityService.AddAsync(Mapper.Map<CommunityModel>(model)));
        }

        //  PUT: HTTP://http://localhost:5001/api/communities
        /// <summary>
        /// Updates community.
        /// </summary>
        /// <param name="model">DTO that encapsulates information needed
        /// for updating of the community.</param>
        /// <returns>Updated model.</returns>
        [HttpPut]
        [Authorize(Policy = "ModeratorOnly")]
        public async Task<IActionResult> Update(UpdateCommunityDto model)
        {
            await _communityService.UpdateAsync(Mapper.Map<CommunityModel>(model));
            return Ok();
        }

        //  DELETE: HTTP://http://localhost:5001/api/communities/1
        /// <summary>
        /// Deletes community.
        /// </summary>
        /// <param name="id">Id of the community to be deleted.</param>
        /// <returns>Task.</returns>
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "ModeratorOnly")]
        public async Task Delete(int id)
        {
            await _communityService.DeleteAsync(id);
        }

        //  POST: HTTP://http://localhost:5001/api/communities/add-moderator?userId=1&communityId=1
        /// <summary>
        /// Adds moderator to the community.
        /// </summary>
        /// <param name="userId">User's id.</param>
        /// <param name="communityId">Community's id.</param>
        /// <returns>Task.</returns>
        [HttpPost("add-moderator")]
        [Authorize(Policy = "ModeratorOnly")]
        public async Task AddModerator(int userId, int communityId)
        {
            await _communityService.AddModeratorAsync(userId, communityId);
        }

        //  DELETE: HTTP://http://localhost:5001/api/communities/remove-moderator?userId=1&communityId=1
        /// <summary>
        /// Removes moderator from the community.
        /// </summary>
        /// <param name="userId">User's id.</param>
        /// <param name="communityId">Community's id.</param>
        /// <returns>Task.</returns>
        [HttpDelete("remove-moderator")]
        [Authorize(Policy = "ModeratorOnly")]
        public async Task RemoveModerator(int userId, int communityId)
        {
            await _communityService.RemoveModeratorAsync(userId, communityId);
        }

        //  POST: HTTP://http://localhost:5001/api/communities/join?userId=1&communityId=1
        /// <summary>
        /// Adds user to the community's members.
        /// </summary>
        /// <param name="userId">User's id.</param>
        /// <param name="communityId">Community's id.</param>
        /// <returns>Task.</returns>
        [HttpPost("join")]
        [Authorize]
        public async Task Join(int userId, int communityId)
        {
            await _communityService.JoinCommunityAsync(userId, communityId);
        }

        //  DELETE: HTTP://http://localhost:5001/api/communities/leave?userId=1&communityId=1
        /// <summary>
        /// Removes user from the community's members.
        /// </summary>
        /// <param name="userId">User's id.</param>
        /// <param name="communityId">Community's id.</param>
        /// <returns>Task.</returns>
        [HttpDelete("leave")]
        [Authorize]
        public async Task Leave(int userId, int communityId)
        {
            await _communityService.LeaveCommunityAsync(userId, communityId);
        }

        //  POST: HTTP://http://localhost:5001/api/communities/add-rule
        /// <summary>
        /// Adds new rule to the community's rules.
        /// </summary>
        /// <param name="model">DTO that encapsulates information needed
        /// for creation of the new rule.</param>
        /// <returns>Task.</returns>
        [HttpPost("add-rule")]
        [Authorize(Policy = "ModeratorOnly")]
        public async Task<IActionResult> AddRule(CreateRuleDto model)
        {
            return Ok(await _ruleService.AddAsync(Mapper.Map<RuleModel>(model)));
        }

        //  PUT: HTTP://http://localhost:5001/api/communities/update-rule
        /// <summary>
        /// Updates community's rule.
        /// </summary>
        /// <param name="model">DTO that encapsulates information needed
        /// for updating of the comment.</param>
        /// <returns>Task.</returns>
        [HttpPut("update-rule")]
        [Authorize(Policy = "ModeratorOnly")]
        public async Task UpdateRule(UpdateRuleDto model)
        {
            await _ruleService.UpdateAsync(Mapper.Map<RuleModel>(model));
        }

        //  DELETE: HTTP://http://localhost:5001/api/communities/delete-rule/1
        /// <summary>
        /// Removes rule from the community's rules.
        /// </summary>
        /// <param name="ruleId">Rule's id.</param>
        /// <returns>Task.</returns>
        [HttpDelete("delete-rule/{ruleId:int}")]
        [Authorize(Policy = "ModeratorOnly")]
        public async Task DeleteRule(int ruleId)
        {
            await _ruleService.DeleteAsync(ruleId);
        }
    }
}
