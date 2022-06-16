using System.Collections.Generic;
using System.Threading.Tasks;
using Business.MapModels;

namespace Business.Interfaces
{
    public interface ICommunityService : ICrud<CommunityModel>
    {
        Task<IEnumerable<UserModel>> GetAllUsers(int communityId);
        Task AddModeratorAsync(int id, int userId);
        Task RemoveModeratorAsync(int id, int userId);
        Task JoinCommunityAsync(int userId, int communityId);
        Task LeaveCommunityAsync(int userId, int communityId);
    }
}
