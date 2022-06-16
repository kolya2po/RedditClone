using System.Threading.Tasks;
using Business.MapModels;

namespace Business.Interfaces
{
    public interface IRuleService
    {
        Task<RuleModel> AddAsync(RuleModel model);

        Task UpdateAsync(RuleModel model);

        Task DeleteAsync(int modelId);
    }
}
