using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    /// <summary>
    /// Contains methods that return entities/enitity with included entities.
    /// </summary>
    /// <typeparam name="TEntity">Entity from subject area.</typeparam>
    public interface IRepositoryExtended<TEntity>
    {
        /// <summary>
        /// Returns asynchronously all entities of type <typeparamref name="TEntity" />  with included entities.
        /// </summary>
        /// <returns>Task that encapsulates collection of entities.</returns>
        Task<IEnumerable<TEntity>> GetAllWithDetailsAsync();

        /// <summary>
        /// Returns entity of type <typeparamref name="TEntity" /> with specified id with included entities.
        /// </summary>
        /// <param name="id">Entity's id.</param>
        /// <returns>Task that encapsulates entity.</returns>
        Task<TEntity> GetByIdWithDetailsAsync(int id);
    }
}
