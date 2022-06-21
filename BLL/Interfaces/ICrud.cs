using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    /// <summary>
    /// Describes CRUD operations with models.
    /// </summary>
    /// <typeparam name="TModel">Mapped model.</typeparam>
    public interface ICrud<TModel> where TModel : class
    {
        /// <summary>
        /// Returns asynchronously all mapped from the entities models of type <typeparamref name="TModel"/>.
        /// </summary>
        /// <returns>Task that encapsulates collection of mapped models.</returns>
        Task<IEnumerable<TModel>> GetAllAsync();

        /// <summary>
        /// Returns asynchronously, mapped the from entity with specified id, model of type <typeparamref name="TModel"/>.
        /// </summary>
        /// <param name="id">Model's id.</param>
        /// <returns>Task that encapsulates mapped model.</returns>
        Task<TModel> GetByIdAsync(int id);

        /// <summary>
        /// Adds asynchronously mapped model of type <typeparamref name="TModel"/> to the table.
        /// </summary>
        /// <param name="model">Model to be added.</param>
        /// <returns>Task that encapsulates mapped model.</returns>
        Task<TModel> AddAsync(TModel model);

        /// <summary>
        /// Updates asynchronously mapped object of type <typeparamref name="TModel"/> in the table.
        /// </summary>
        /// <param name="model">Model to be updated.</param>
        /// <returns>Task.</returns>
        Task UpdateAsync(TModel model);


        /// <summary>
        /// Deletes asynchronously model of type <typeparamref name="TModel"/> with specified id from the table.
        /// </summary>
        /// <param name="modelId">Model's id.</param>
        /// <returns>Task.</returns>
        Task DeleteAsync(int modelId);
    }
}
