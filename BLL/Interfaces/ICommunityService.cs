using System.Collections.Generic;
using System.Threading.Tasks;
using Business.MapModels;

namespace Business.Interfaces
{
    /// <summary>
    /// Describes CRUD operations with CommunityModels and functionality for users to
    /// interact with community.
    /// </summary>
    public interface ICommunityService : ICrud<CommunityModel>
    {
        /// <summary>
        /// Returns asynchronously all mapped from users entities models of type UserModel that belong to the community.
        /// </summary>
        /// <param name="communityId">Community's id.</param>
        /// <returns>Task that encapsulates collection of mapped models.</returns>
        Task<IEnumerable<UserModel>> GetAllUsers(int communityId);

        /// <summary>
        /// Adds asynchronously the moderator to the community.
        /// </summary>
        /// <param name="id">Community's id.</param>
        /// <param name="userId">Moderator's id.</param>
        /// <returns>Task.</returns>
        Task AddModeratorAsync(int id, int userId);

        /// <summary>
        /// Removes asynchronously the moderator from the community.
        /// </summary>
        /// <param name="id">Community's id.</param>
        /// <param name="userId">Moderator's id.</param>
        /// <returns>Task.</returns>
        Task RemoveModeratorAsync(int id, int userId);

        /// <summary>
        /// Joins asynchronously the user to the community.
        /// </summary>
        /// <param name="userId">User's id.</param>
        /// <param name="communityId">Community's id.</param>
        /// <returns>Task.</returns>
        Task JoinCommunityAsync(int userId, int communityId);

        /// <summary>
        /// Excludes asynchronously the user from the community.
        /// </summary>
        /// <param name="userId">User's id.</param>
        /// <param name="communityId">Community's id/</param>
        /// <returns>Task.</returns>
        Task LeaveCommunityAsync(int userId, int communityId);
    }
}
