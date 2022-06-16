using Data.Models;

namespace Data.Interfaces
{
    /// <inheritdoc cref="IRepository{Community}"/>
    public interface ICommunityRepository : IRepository<Community>, IRepositoryExtended<Community>
    {
    }
}
