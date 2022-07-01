using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.MapModels;
using Forum.WebApi.Models.Rule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.WebApi.Controllers
{
    /// <summary>
    /// Represents controller that handles requests related to communities.
    /// </summary>
    [ApiController]
    [Route("api/communities")]
    public class RulesController : BaseController
    {
        private readonly IRuleService _ruleService;

        /// <summary>
        /// Initializes new instance of the CommunitiesController class and class's fields.
        /// </summary>
        /// <param name="mapper">>Instance of Mapper class.</param>
        /// <param name="ruleService">>Instance of RuleService class.</param>
        public RulesController(IMapper mapper, IRuleService ruleService) : base(mapper)
        {
            _ruleService = ruleService;
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
