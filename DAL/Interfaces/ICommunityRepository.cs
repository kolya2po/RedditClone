using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;

namespace Data.Interfaces
{
    public interface ICommunityRepository : IRepository<Community>
    {
        Task<IEnumerable<Community>> GetAllWithDetailsAsync();

        Task<Community> GetByIdWithDetailsAsync(int id);
    }
}
