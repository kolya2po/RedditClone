using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;

namespace Data.Interfaces
{
    public interface IUserCommunityRepository
    {
        Task<IEnumerable<UserCommunity>> GetAllAsync();

        Task<UserCommunity> GetByIdAsync(int id);

        Task AddAsync(UserCommunity entity);

        void Delete(UserCommunity entity);

        Task DeleteByIdAsync(int id);

        void Update(UserCommunity entity);
        Task<IEnumerable<UserCommunity>> GetAllWithDetailsAsync();

        Task<UserCommunity> GetByIdWithDetailsAsync(int id);
    }
}
