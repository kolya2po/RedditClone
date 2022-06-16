using Data.Models;

namespace Data.Interfaces
{
    /// <inheritdoc cref="IRepository{Comment}"/>
    /// <inheritdoc cref="IRepositoryExtended{Comment}"/>
    public interface ICommentRepository : IRepository<Comment>, IRepositoryExtended<Comment>
    {
    }
}
