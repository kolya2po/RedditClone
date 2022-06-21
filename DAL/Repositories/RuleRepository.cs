using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    /// <summary>
    /// Extends BaseRepository class and implements IRuleRepository interface.
    /// </summary>
    public class RuleRepository : BaseRepository, IRuleRepository
    {
        /// <inheritdoc/>
        public RuleRepository(ForumDbContext dbContext) : base(dbContext)
        {
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Rule>> GetAllAsync()
        {
            return await DbContext.Rules.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Rule> GetByIdAsync(int id)
        {
            return await DbContext.Rules.FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <inheritdoc />
        public async Task AddAsync(Rule entity)
        {
            entity.Community = null;
            await DbContext.Rules.AddAsync(entity);
        }

        /// <inheritdoc />
        public void Delete(Rule entity)
        {
            DbContext.Rules.Remove(entity);
        }

        /// <inheritdoc />
        public async Task DeleteByIdAsync(int id)
        {
            var rule = await DbContext.Rules.FirstOrDefaultAsync(r => r.Id == id);

            if (rule != null)
            {
                DbContext.Rules.Remove(rule);
            }
        }

        /// <inheritdoc />
        public void Update(Rule entity)
        {
            entity.Community = null;
            DbContext.Rules.Update(entity);
        }
    }
}
