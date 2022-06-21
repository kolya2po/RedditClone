using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.MapModels;
using Business.MapModels.Identity;
using Forum.WebApi.Models.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IMapper mapper, IUserService userService) : base(mapper)
        {
            _userService = userService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _userService.GetByIdAsync(id));
        }

        [HttpPost("registration")]
        public async Task<UserDto> Registration(RegistrationModelDto model)
        {
            return await _userService.RegistrationAsync(Mapper.Map<RegistrationModel>(model));
        }

        [HttpPost("login")]
        public async Task<UserDto> Login(LoginModelDto model)
        {
            return await _userService.LoginAsync(Mapper.Map<LoginModel>(model));
        }

        [HttpGet]
        public async Task Logout()
        {
            await _userService.LogoutAsync();
        }

        [HttpPut]
        public async Task Update(UpdateUserDto model)
        {
            await _userService.UpdateAsync(Mapper.Map<UserModel>(model));
        }
    }
}
