using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.MapModels;
using Business.Validation;
using Data.Interfaces;
using Data.Models;

namespace Business.Services
{
    public class RuleService : BaseService, IRuleService
    {
        /// <inheritdoc />
        public RuleService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        /// <inheritdoc />
        public async Task<IEnumerable<RuleModel>> GetAllByCommunityIdAsync(int communityId)
        {
            var community = await UnitOfWork.CommunityRepository.GetByIdWithDetailsAsync(communityId);

            if (community == null)
            {
                throw new ForumException("Community doesn't exist");
            }

            return Mapper.Map<IEnumerable<RuleModel>>(community.Rules);
        }

        /// <inheritdoc />
        public async Task AddAsync(RuleModel model)
        {
            if (string.IsNullOrEmpty(model.Title))
            {
                throw new ForumException("Title cannot be null");
            }

            await UnitOfWork.RuleRepository.AddAsync(Mapper.Map<Rule>(model));
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        public async Task UpdateAsync(RuleModel model)
        {
            if (model == null)
            {
                throw new ForumException("Rule cannot be null");
            }
            if (string.IsNullOrEmpty(model.Title))
            {
                throw new ForumException("Title cannot be null");
            }

            UnitOfWork.RuleRepository.Update(Mapper.Map<Rule>(model));
            await UnitOfWork.SaveAsync();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(int modelId)
        {
            await UnitOfWork.RuleRepository.DeleteByIdAsync(modelId);
            await UnitOfWork.SaveAsync();
        }
    }
}
