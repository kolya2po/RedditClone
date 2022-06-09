using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;

namespace Data.Interfaces
{
    public interface IUserCommunityRepository : IRepository<UserCommunity>
    {
        Task<IEnumerable<UserCommunity>> GetAllWithDetailsAsync();

        Task<UserCommunity> GetByIdWithDetailsAsync(int id);
    }
}
