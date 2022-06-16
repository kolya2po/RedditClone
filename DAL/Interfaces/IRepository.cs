using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    /// <summary>
    /// Contains base operations that can by performed on table.
    /// </summary>
    /// <typeparam name="TEntity">Entity from subject area.</typeparam>
    public interface IRepository<TEntity>
    {
        /// <summary>
        /// Returns asynchronously all records of type <typeparamref name="TEntity" /> from the table.
        /// </summary>
        /// <returns>Task that encapsulates collection of entities.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Returns asynchronously entity of type <typeparamref name="TEntity" /> with specified id.
        /// </summary>
        /// <param name="id">Entity's id.</param>
        /// <returns>Task that encapsulates entity</returns>
        Task<TEntity> GetByIdAsync(int id);

        /// <summary>
        /// Adds asynchronously new entity of type <typeparamref name="TEntity" /> to the table.
        /// </summary>
        /// <param name="entity">Entity to be added.</param>
        /// <returns>Task.</returns>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// Deletes entity of type <typeparamref name="TEntity" /> from the table.
        /// </summary>
        /// <param name="entity">Entity to be deleted.</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Deletes asynchronously entity of type <typeparamref name="TEntity" /> with specified id from the table.
        /// </summary>
        /// <param name="id">Entity's id.</param>
        /// <returns>Task.</returns>
        Task DeleteByIdAsync(int id);

        /// <summary>
        /// Updates entity of type <typeparamref name="TEntity" /> in the table.
        /// </summary>
        /// <param name="entity">Entity to be updated.</param>
        void Update(TEntity entity);
    }
}
