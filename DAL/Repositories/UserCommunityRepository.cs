using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    /// <summary>
    /// Extends BaseRepository class and implements IUserCommunityRepository interface.
    /// </summary>
    public class UserCommunityRepository : BaseRepository, IUserCommunityRepository
    {
        /// <inheritdoc />
        public UserCommunityRepository(ForumDbContext dbContext) : base(dbContext)
        {
        }

        /// <inheritdoc />
        public async Task<IEnumerable<UserCommunity>> GetAllAsync()
        {
            return await DbContext.UserCommunities.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<UserCommunity> GetByIdAsync(int id)
        {
            return await DbContext.UserCommunities.FirstOrDefaultAsync(uc => uc.Id == id);
        }

        /// <inheritdoc />
        public async Task AddAsync(UserCommunity entity)
        {
            await DbContext.UserCommunities.AddAsync(entity);
        }

        /// <inheritdoc />
        public void Delete(UserCommunity entity)
        {
            DbContext.UserCommunities.Remove(entity);
        }

        /// <inheritdoc />
        public async Task DeleteByIdAsync(int id)
        {
            var uc = await DbContext.UserCommunities.FirstOrDefaultAsync(uc => uc.Id == id);

            if (uc != null)
            {
                DbContext.UserCommunities.Remove(uc);
            }
        }

        /// <inheritdoc />
        public void Update(UserCommunity entity)
        {
            DbContext.UserCommunities.Update(entity);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<UserCommunity>> GetAllWithDetailsAsync()
        {
            return await DbContext.UserCommunities
                .Include(uc => uc.User)
                .Include(uc => uc.Community)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<UserCommunity> GetByIdWithDetailsAsync(int id)
        {
            return await DbContext.UserCommunities
                .Include(uc => uc.User)
                .Include(uc => uc.Community)
                .FirstOrDefaultAsync(uc => uc.Id == id);
        }
    }
}
