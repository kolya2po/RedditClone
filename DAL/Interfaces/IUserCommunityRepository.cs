using Data.Models;

namespace Data.Interfaces
{
    /// <inheritdoc cref="IRepository{UserCommunity}"/>
    /// <inheritdoc cref="IRepositoryExtended{UserCommunity}"/>
    public interface IUserCommunityRepository : IRepository<UserCommunity>, IRepositoryExtended<UserCommunity>
    {

    }
}
