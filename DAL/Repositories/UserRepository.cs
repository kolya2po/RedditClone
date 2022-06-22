using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    /// <summary>
    /// Represents repository that encapsulates operations with users db table.
    /// </summary>
    public class UserRepository : BaseRepository, IUserRepository
    {
        /// <inheritdoc />
        public UserRepository(ForumDbContext dbContext) : base(dbContext)
        {
        }

        /// <inheritdoc />
        public async Task<IEnumerable<User>> GetAllWithDetailsAsync()
        {
            return await DbContext.Users
                .Include(u => u.Comments)
                .Include(u => u.Posts)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<User> GetByIdWithDetailsAsync(int id)
        {
            return await DbContext.Users
                .Include(u => u.Comments)
                .ThenInclude(c => c.Topic)
                .Include(u => u.Posts)
                .ThenInclude(t => t.Community)
                .Include(u => u.Communities)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
