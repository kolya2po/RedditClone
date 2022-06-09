using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;

namespace Data.Interfaces
{
    public interface ITopicRepository : IRepository<Topic>
    {
        Task<IEnumerable<Topic>> GetAllWithDetailsAsync();

        Task<Topic> GetByIdWithDetailsAsync(int id);
    }
}
