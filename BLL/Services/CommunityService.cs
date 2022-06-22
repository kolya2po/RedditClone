using AutoMapper;
using Business.Interfaces;
using Business.MapModels;
using Business.Validation;
using Data.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Business.Services
{
    /// <summary>
    /// Represents service that works with communities.
    /// </summary>
    public class CommunityService : BaseService, ICommunityService
    {
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Initializes a new instance of the CommunityService.
        /// Also initializes class's fields.
        /// </summary>
        /// <param name="unitOfWork">Instance of the UnitOfWork class.</param>
        /// <param name="mapper">Instance of the Mapper class.</param>
        /// <param name="userManager">Instance of the UserManager&lt;int&gt; class.</param>

        public CommunityService(IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper) : base(unitOfWork, mapper)
        {
            _userManager = userManager;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CommunityModel>> GetAllAsync()
        {
            var communities = await UnitOfWork.CommunityRepository.GetAllWithDetailsAsync();

            return Mapper.Map<IEnumerable<CommunityModel>>(communities);
        }

        /// <summary>
        /// Returns asynchronously, mapped the from entity with specified id, model of type CommunityModel.
        /// Sets some of model's fields to null, because of optimization.
        /// Those fields aren't needed on the UI.
        /// </summary>
        /// <param name="id">Model's id.</param>
        /// <returns>Task that encapsulates mapped model.</returns>
        public async Task<CommunityModel> GetByIdAsync(int id)
        {
            var community = await UnitOfWork.CommunityRepository.GetByIdWithDetailsAsync(id);

            var model = Mapper.Map<CommunityModel>(community);

            foreach (var topic in model.PostModels)
            {
                topic.CommentModels = null;
            }

            foreach (var moderator in model.ModeratorModels)
            {
                moderator.PostModels = null;
                moderator.CommentModels = null;
            }

            return model;
        }

        /// <inheritdoc />
        /// <exception cref="ForumException">Throws if user doesn't exist.</exception>
        /// <exception cref="ForumException">Throws if user has already created community.</exception>
        /// <exception cref="ForumException">Throws if community with the same title already exist.</exception>
        public async Task<CommunityModel> AddAsync(CommunityModel model)
        {
            var user = await _userManager.FindByIdAsync(model.CreatorId.ToString());

            if (user == null)
            {
                throw new ForumException("User doesn't exist.");
            }

            if (user.CreatedCommunityId != null)
            {
                throw new ForumException("User has already created community.");
            }

            model.CreationDate = DateTime.Now.ToLongDateString();
            await UnitOfWork.CommunityRepository.AddAsync(Mapper.Map<Community>(model));

            try
            {
                await UnitOfWork.SaveAsync();
            }
            catch (DbUpdateException)
            {
                throw new ForumException("Community with the same title already exist.");
            }

            await AttachUserToCreatedCommunityAsync(user, model.Title);
            return model;
        }

        /// <inheritdoc />
        public async Task UpdateAsync(CommunityModel model)
        {
            UnitOfWork.CommunityRepository.Update(Mapper.Map<Community>(model));
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        /// <exception cref="ForumException">Throws if user doesn't exist.</exception>
        public async Task DeleteAsync(int modelId)
        {
            var community = await UnitOfWork.CommunityRepository.GetByIdAsync(modelId);
            var user = await _userManager.FindByIdAsync(community.CreatorId.ToString());

            if (user == null)
            {
                throw new ForumException("User doesn't exist.");
            }

            var claim = new Claim("ModeratorRole", nameof(Roles.Moderator));
            await _userManager.RemoveClaimAsync(user, claim);

            await UnitOfWork.CommunityRepository.DeleteByIdAsync(modelId);
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        /// <exception cref="ForumException">Throws if community doesn't exist.</exception>
        public async Task<IEnumerable<UserModel>> GetAllUsersAsync(int communityId)
        {
            var community = await UnitOfWork.CommunityRepository.GetByIdWithDetailsAsync(communityId);

            if (community == null)
            {
                throw new ForumException("Community doesn't exist.");
            }

            var users = Mapper.Map<IEnumerable<UserModel>>(community.Members.Select(c => c.User)).ToList();

            foreach (var userModel in users)
            {
                userModel.CommentModels = null;
                userModel.PostModels = null;
            }

            return users;
        }

        /// <inheritdoc />
        /// <exception cref="ForumException">Throws if user doesn't exist.</exception>
        ///  /// <exception cref="ForumException">Throws if user already moderates community.</exception>
        public async Task AddModeratorAsync(int userId, int communityId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new ForumException("User doesn't exist.");
            }

            if (user.CreatedCommunityId != null)
            {
                throw new ForumException("User can moderate only one community.");
            }

            var claim = new Claim(ClaimTypes.Role, nameof(Roles.Moderator));
            await _userManager.AddClaimAsync(user, claim);

            user.ModeratedCommunityId = communityId;
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        public async Task RemoveModeratorAsync(int userId, int communityId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user != null && user.ModeratedCommunityId == communityId)
            {
                var claim = new Claim(ClaimTypes.Role, "Moderator");
                await _userManager.RemoveClaimAsync(user, claim);

                user.ModeratedCommunityId = null;
                await UnitOfWork.SaveAsync();
            }
        }

        /// <inheritdoc />
        public async Task JoinCommunityAsync(int userId, int communityId)
        {
            var existedUserCommunity = (await UnitOfWork.UserCommunityRepository.GetAllAsync())
                .FirstOrDefault(uc => uc.UserId == userId && uc.CommunityId == communityId);

            if (existedUserCommunity != null)
            {
                return;
            }

            var userCommunity = new UserCommunity
            {
                UserId = userId,
                CommunityId = communityId
            };

            await UnitOfWork.UserCommunityRepository.AddAsync(userCommunity);
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        public async Task LeaveCommunityAsync(int userId, int communityId)
        {
            var userCommunity = (await UnitOfWork.UserCommunityRepository.GetAllAsync())
                .FirstOrDefault(uc => uc.UserId == userId && uc.CommunityId == communityId);

            if (userCommunity != null)
            {
                UnitOfWork.UserCommunityRepository.Delete(userCommunity);
                await UnitOfWork.SaveAsync();
            }
        }

        private async Task AttachUserToCreatedCommunityAsync(User user, string communityTitle)
        {
            var claim = new Claim(ClaimTypes.Role, nameof(Roles.Moderator));
            await _userManager.AddClaimAsync(user, claim);

            var community = (await UnitOfWork.CommunityRepository.GetAllAsync())
                .First(c => c.Title == communityTitle);

            user.CreatedCommunityId = community.Id;
            user.ModeratedCommunityId = community.Id;
            var userCommunity = new UserCommunity { UserId = user.Id, CommunityId = community.Id };
            await UnitOfWork.UserCommunityRepository.AddAsync(userCommunity);
            await UnitOfWork.SaveAsync();
        }
    }
}
