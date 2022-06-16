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
using System.Threading.Tasks;

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
        public async Task<CommunityModel> AddAsync(CommunityModel model)
        {
            if (model == null)
            {
                throw new ForumException("Passed model was equal null.");
            }

            var user = await _userManager.FindByIdAsync(model.CreatorId.ToString());

            if (user == null)
            {
                throw new ForumException("User doesn't exist.");
            }

            model.CreationDate = DateTime.Now.ToLongTimeString();
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
            if (model == null)
            {
                throw new ForumException("Passed model was equal null.");
            }

            UnitOfWork.CommunityRepository.Update(Mapper.Map<Community>(model));
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(int modelId)
        {
            var community = await UnitOfWork.CommunityRepository.GetByIdAsync(modelId);
            var user = await _userManager.FindByIdAsync(community.CreatorId.ToString());

            if (user == null)
            {
                throw new ForumException("User doesn't exist.");
            }

            var role = await _roleManager.FindByNameAsync("Moderator");
            await _userManager.RemoveFromRoleAsync(user, role.Name);

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

            return Mapper.Map<IEnumerable<UserModel>>(community.Members.Select(c => c.User));
        }

        /// <inheritdoc />
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

            var role = await _roleManager.FindByNameAsync("Moderator");
            await _userManager.AddToRoleAsync(user, role.Name);

            user.ModeratedCommunityId = communityId;
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        public async Task RemoveModeratorAsync(int userId, int communityId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user != null && user.ModeratedCommunityId == communityId)
            {
                var role = await _roleManager.FindByNameAsync("Moderator");
                await _userManager.RemoveFromRoleAsync(user, role.Name);

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
            var role = await _roleManager.FindByNameAsync("Moderator");
            await _userManager.AddToRoleAsync(user, role.Name);

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
