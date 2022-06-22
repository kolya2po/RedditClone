using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    /// <summary>
    /// Represents repository that encapsulates operations with communities db table.
    /// </summary>
    public class CommunityRepository : BaseRepository, ICommunityRepository
    {
        /// <inheritdoc />
        public CommunityRepository(ForumDbContext dbContext) : base(dbContext)
        {
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Community>> GetAllAsync()
        {
            return await DbContext.Communities.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Community> GetByIdAsync(int id)
        {
            return await DbContext.Communities.FirstOrDefaultAsync(c => c.Id == id);
        }

        /// <inheritdoc />
        public async Task AddAsync(Community entity)
        {
            entity.Creator = null;
            await DbContext.Communities.AddAsync(entity);
        }

        /// <inheritdoc />
        public void Delete(Community entity)
        {
            DbContext.Communities.Remove(entity);
        }

        /// <inheritdoc />
        public async Task DeleteByIdAsync(int id)
        {
            var community = await DbContext.Communities.FirstOrDefaultAsync(c => c.Id == id);

            if (community != null)
            {
                DbContext.Communities.Remove(community);
            }
        }

        /// <inheritdoc />
        public void Update(Community entity)
        {
            entity.Creator = null;
            DbContext.Communities.Update(entity);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Community>> GetAllWithDetailsAsync()
        {
            return await DbContext.Communities
                .Include(c => c.Creator)
                .Include(c => c.Members)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Community> GetByIdWithDetailsAsync(int id)
        {
            return await DbContext.Communities
                .Include(c => c.Posts)
                .ThenInclude(p => p.Author)
                .Include(c => c.Posts)
                .ThenInclude(p => p.Comments)
                .Include(c => c.Rules)
                .Include(c => c.Members)
                .Include(c => c.Moderators)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
