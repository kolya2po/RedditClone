using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;

namespace Data.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetAllWithDetailsAsync();
        Task<Comment> GetByIdWithDetailsAsync(int id);
    }
}
