using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.MapModels;
using Data.Interfaces;
using Data.Models;

namespace Business.Services
{
    /// <summary>
    /// Represents service that works with rules.
    /// </summary>
    public class RuleService : BaseService, IRuleService
    {
        /// <inheritdoc />
        public RuleService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        /// <inheritdoc />
        public async Task<RuleModel> AddAsync(RuleModel model)
        {
            await UnitOfWork.RuleRepository.AddAsync(Mapper.Map<Rule>(model));
            await UnitOfWork.SaveAsync();

            return model;
        }

        /// <inheritdoc />
        public async Task UpdateAsync(RuleModel model)
        {
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
