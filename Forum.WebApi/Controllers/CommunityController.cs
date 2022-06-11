using System.Threading.Tasks;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommunityController : ControllerBase
    {
        private readonly CommunityService _communityService;
        public CommunityController(CommunityService communityService)
        {
            _communityService = communityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _communityService.GetAllAsync());
        }

        [HttpGet("users/{communityId:int}")]
        [Authorize(Roles = "Moderator, Administrator")]
        public async Task<IActionResult> GetAllUsers(int communityId)
        {
            return Ok(await _communityService.GetAllUsers(communityId));
        }
    }
}
