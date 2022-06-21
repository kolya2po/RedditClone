using System.Threading.Tasks;
using Business.MapModels;

namespace Business.Interfaces
{
    /// <summary>
    /// Describes CUD operations with RuleModel entities.
    /// </summary>
    public interface IRuleService
    {
        /// <inheritdoc cref="ICrud{TModel}.AddAsync"/>
        Task<RuleModel> AddAsync(RuleModel model);

        /// <inheritdoc cref="ICrud{TModel}.UpdateAsync"/>
        Task UpdateAsync(RuleModel model);

        /// <inheritdoc cref="ICrud{TModel}.DeleteAsync"/>
        Task DeleteAsync(int modelId);
    }
}
