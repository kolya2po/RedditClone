using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
    /// <summary>
    /// Represents service that works with users.
    /// </summary>
    public class UserService : BaseService, IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtHandler _jwnHandler;

        /// <summary>
        /// Initializes new instance of the UserService class.
        /// </summary>
        /// <param name="unitOfWork">Unit of work.</param>
        /// <param name="userManager">Identity's user manager.</param>
        /// <param name="signInManager">Identity's sign in manager.</param>
        /// <param name="mapper">Instance that implements IMapper interface.</param>
        /// <param name="jwnHandler">Instance that implements IJwtHandler interface.</param>
        public UserService(IUnitOfWork unitOfWork, UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IJwtHandler jwnHandler) : base(unitOfWork, mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwnHandler = jwnHandler;
        }

        /// <inheritdoc />
        /// <exception cref="ForumException">Throws if user with the same email already exists.</exception>
        public async Task<UserDto> RegistrationAsync(RegistrationModel model)
        {
            IdentityResult result;

            var user = new User
            {
                Email = model.Email,
                UserName = model.UserName,
                BirthDate = model.BirthDate
            };

            try
            {
                result = await _userManager.CreateAsync(user, model.Password);
            }
            catch (DbUpdateException)
            {
                throw new ForumException("User with the same email already exists.");
            }

            if (!result.Succeeded)
            {
                return null;
            }

            await _signInManager.SignInAsync(user, false);

            await _userManager.AddClaimsAsync(user, new[]
            {
                new Claim(ClaimTypes.Role, nameof(Roles.Registered))
            });

            var claims = await _userManager.GetClaimsAsync(user);

            var token = GetToken(claims);

            return new UserDto { Id = user.Id, Token = token };
        }

        /// <inheritdoc />
        /// <exception cref="ForumException">Throws if user doesn't exist.</exception>
        /// <exception cref="ForumException">Throws if user has input wrong credentials.</exception>
        public async Task<UserDto> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                throw new ForumException("User doesn't exist.");
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password,
                false, false);

            if (!result.Succeeded)
            {
                throw new ForumException("Incorrect user's data.");
            }

            var claims = await _userManager.GetClaimsAsync(user);

            var token = GetToken(claims);

            return new UserDto { Id = user.Id, Token = token };
        }

        /// <inheritdoc />
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        /// <inheritdoc />
        /// <exception cref="ForumException">Throws if user doesn't exist.</exception>
        public async Task<UserModel> GetByIdAsync(int userId)
        {
            var user = await UnitOfWork.UserRepository.GetByIdWithDetailsAsync(userId);

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

            var claims = await _userManager.GetClaimsAsync(user);
            var token = GetToken(claims);

            var model = Mapper.Map<UserModel>(user);
            model.Token = token;

            foreach (var topicModel in model.PostModels)
            {
                topicModel.CommentModels = null;
            }

            return model;
        }

        /// <inheritdoc />
        public async Task UpdateAsync(UserModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());

            user.UserName = model.UserName;
            user.BirthDate = Convert.ToDateTime(model.BirthDate);

            await _userManager.UpdateAsync(user);
        }

        private string GetToken(IEnumerable<Claim> claims)
        {
            var signInCredentials = _jwnHandler.GetSigningCredentials();
            var tokenOptions = _jwnHandler.GenerateTokenOptions(signInCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return token;
        }
    }
}
