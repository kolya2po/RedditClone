using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.MapModels;
using Business.MapModels.Identity;
using Business.Validation;
using Data.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        public UserService(IUnitOfWork unitOfWork, UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, RoleManager<IdentityRole<int>> roleManager) : base(unitOfWork, mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        /// <inheritdoc />
        public async Task<bool> RegistrationAsync(RegistrationModel model)
        {
            IdentityResult result;

            var user = new User
            {
                Email = model.Email,
                UserName = model.UserName
            };

            try
            {
                result = await _userManager.CreateAsync(user, model.Password);
            }
            catch (DbUpdateException)
            {
                throw new ForumException("User with the same email already exist.");
            }

            if (!result.Succeeded)
            {
                return false;
            }

            var role = await _roleManager.FindByNameAsync("Registered");
            if (role != null)
            {
                await _userManager.AddToRoleAsync(user, role.Name);
            }
            await _signInManager.SignInAsync(user, false);

            return true;
        }

        /// <inheritdoc />
        public async Task<bool> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var result = await _signInManager.PasswordSignInAsync(user, model.Password,
                model.RememberMe, false);

            return result.Succeeded;
        }

        /// <inheritdoc />
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        /// <inheritdoc />
        public async Task<UserModel> GetByIdAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new ForumException("User doesn't exist.");
            }

            var topicsRating = (await UnitOfWork.TopicRepository.GetAllAsync())
                .Where(p => p.AuthorId == userId)
                .Sum(p => p.Rating);

            var commentsRating = (await UnitOfWork.CommentRepository.GetAllAsync())
                .Where(c => c.AuthorId == userId)
                .Sum(c => c.Rating);

            user.Karma = topicsRating + commentsRating;

            return Mapper.Map<UserModel>(user);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(UserModel model)
        {
            var user = Mapper.Map<User>(model);
            await _userManager.UpdateAsync(user);
            await UnitOfWork.SaveAsync();
        }
    }
}
