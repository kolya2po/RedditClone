using Data.Models;

namespace Data.Interfaces
{
    /// <inheritdoc cref="IRepository{Topic}"/>
    /// <inheritdoc cref="IRepositoryExtended{Topic}"/>
    public interface ITopicRepository : IRepository<Topic>, IRepositoryExtended<Topic>
    {
    }
}
