using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.MapModels;
using Business.MapModels.Identity;
using Forum.WebApi.Models.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum.WebApi.Controllers
{
    /// <summary>
    /// Represents controller that handles requests related to users.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes new instance of the UsersController class and class's fields.
        /// </summary>
        /// <param name="mapper">Instance of Mapper class.</param>
        /// <param name="userService">Instance of UserService class.</param>
        public UsersController(IMapper mapper, IUserService userService) : base(mapper)
        {
            _userService = userService;
        }

        //  GET: HTTP://http://localhost:5001/api/users/1
        /// <summary>
        /// Returns user with specified id.
        /// </summary>
        /// <param name="id">User's id.</param>
        /// <returns>User model with specified id.</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _userService.GetByIdAsync(id));
        }

        //  POST: HTTP://http://localhost:5001/api/users/registration
        /// <summary>
        /// Registers new application's user.
        /// </summary>
        /// <param name="model">DTO that encapsulates information needed
        /// for creating of the new user.</param>
        /// <returns>Model of created user.</returns>
        [HttpPost("registration")]
        public async Task<UserDto> Registration(RegistrationModelDto model)
        {
            return await _userService.RegistrationAsync(Mapper.Map<RegistrationModel>(model));
        }

        //  POST: HTTP://http://localhost:5001/api/users/login
        /// <summary>
        /// Logs user in.
        /// </summary>
        /// <param name="model">DTO that encapsulates information needed
        /// for letting user in the system.</param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<UserDto> Login(LoginModelDto model)
        {
            return await _userService.LoginAsync(Mapper.Map<LoginModel>(model));
        }

        //  GET: HTTP://http://localhost:5001/api/users
        /// <summary>
        /// Logs user out.
        /// </summary>
        /// <returns>Task.</returns>
        [HttpGet]
        public async Task Logout()
        {
            await _userService.LogoutAsync();
        }

        //  PUT: HTTP://http://localhost:5001/api/users
        /// <summary>
        /// Updates user in the db table.
        /// </summary>
        /// <param name="model">DTO that encapsulates information needed
        /// for updating of the user.</param>
        /// <returns>Task.</returns>
        [HttpPut]
        public async Task Update(UpdateUserDto model)
        {
            await _userService.UpdateAsync(Mapper.Map<UserModel>(model));
        }
    }
}
