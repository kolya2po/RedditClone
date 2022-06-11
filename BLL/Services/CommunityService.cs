using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.MapModels;
using Business.Validation;
using Data.Interfaces;
using Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Business.Services
{
    public class CommunityService : BaseService, ICommunityService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        /// <inheritdoc />
        public CommunityService(IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper, RoleManager<IdentityRole<int>> roleManager) : base(unitOfWork, mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<CommunityModel>> GetAllAsync()
        {
            var communities = await UnitOfWork.CommunityRepository.GetAllWithDetailsAsync();

            return Mapper.Map<IEnumerable<CommunityModel>>(communities);
        }

        /// <inheritdoc />
        public async Task<CommunityModel> GetByIdAsync(int id)
        {
            var community = await UnitOfWork.CommunityRepository.GetByIdWithDetailsAsync(id);

            return Mapper.Map<CommunityModel>(community);
        }

        /// <inheritdoc />
        public async Task AddAsync(CommunityModel model)
        {
            if (model == null)
            {
                throw new ForumException("Passed model was equal null.");
            }
            if (string.IsNullOrEmpty(model.Title))
            {
                throw new ForumException("Title cannot be null.");
            }

            var user = await _userManager.FindByIdAsync(model.CreatorId.ToString());
            var role = await _roleManager.FindByNameAsync("Moderator");
            await _userManager.AddToRoleAsync(user, role.Name);

            model.CreationDate = DateTime.Now;
            await UnitOfWork.CommunityRepository.AddAsync(Mapper.Map<Community>(model));
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        public async Task UpdateAsync(CommunityModel model)
        {
            if (model == null)
            {
                throw new ForumException("Passed model was equal null.");
            }
            if (string.IsNullOrEmpty(model.Title))
            {
                throw new ForumException("Title cannot be null.");
            }

            UnitOfWork.CommunityRepository.Update(Mapper.Map<Community>(model));
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(int modelId)
        {
            await UnitOfWork.CommunityRepository.DeleteByIdAsync(modelId);
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<UserModel>> GetAllUsers(int communityId)
        {
            var community = await UnitOfWork.CommunityRepository.GetByIdWithDetailsAsync(communityId);

            if (community == null)
            {
                throw new ForumException("Community doesn't exist.");
            }

            return Mapper.Map<IEnumerable<UserModel>>(community.Members);
        }

        /// <inheritdoc />
        public async Task AddModeratorAsync(int userId, int communityId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user != null)
            {
                var role = await _roleManager.FindByNameAsync("Moderator");
                await _userManager.AddToRoleAsync(user, role.Name);

                user.ModeratedCommunityId = communityId;
                await _userManager.UpdateAsync(user);
                await UnitOfWork.SaveAsync();
            }
        }

        /// <inheritdoc />
        public async Task RemoveModeratorAsync(int userId, int communityId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user != null)
            {
                var role = await _roleManager.FindByNameAsync("Moderator");
                await _userManager.RemoveFromRoleAsync(user, role.Name);

                user.ModeratedCommunityId = null;
                await _userManager.UpdateAsync(user);
                await UnitOfWork.SaveAsync();
            }
        }

        /// <inheritdoc />
        public async Task JoinCommunity(int userId, int communityId)
        {
            var userCommunity = new UserCommunity
            {
                UserId = userId,
                CommunityId = communityId
            };

            await UnitOfWork.UserCommunityRepository.AddAsync(userCommunity);
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        public async Task LeaveCommunity(int userId, int communityId)
        {
            var userCommunity = (await UnitOfWork.UserCommunityRepository.GetAllAsync())
                .FirstOrDefault(uc => uc.UserId == userId && uc.CommunityId == communityId);

            if (userCommunity != null)
            {
                UnitOfWork.UserCommunityRepository.Delete(userCommunity);
                await UnitOfWork.SaveAsync();
            }
        }
    }
}
