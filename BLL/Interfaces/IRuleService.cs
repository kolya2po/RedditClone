using System.Collections.Generic;
using System.Threading.Tasks;
using Business.MapModels;

namespace Business.Interfaces
{
    public interface IRuleService
    {
        Task<IEnumerable<RuleModel>> GetAllByCommunityIdAsync(int communityId);
        Task AddAsync(RuleModel model);

        Task UpdateAsync(RuleModel model);

        Task DeleteAsync(int modelId);
    }
}
