using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    /// <summary>
    /// Extends BaseRepository class and implements ICommentRepository interface.
    /// </summary>
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        /// <inheritdoc />
        public CommentRepository(ForumDbContext dbContext) : base(dbContext)
        {
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await DbContext.Comments.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Comment> GetByIdAsync(int id)
        {
            return await DbContext.Comments.FirstOrDefaultAsync(c => c.Id == id);
        }

        /// <inheritdoc />
        public async Task AddAsync(Comment entity)
        {
            entity.Author = null;
            entity.Topic = null;
            await DbContext.Comments.AddAsync(entity);
        }

        /// <inheritdoc />
        public void Delete(Comment entity)
        {
            DbContext.Comments.Remove(entity);
        }

        /// <inheritdoc />
        public async Task DeleteByIdAsync(int id)
        {
            var comment = await DbContext.Comments.FirstOrDefaultAsync(c => c.Id == id);

            if (comment != null)
            {
                DbContext.Comments.Remove(comment);
            }
        }

        /// <inheritdoc />
        public void Update(Comment entity)
        {
            entity.Author = null;
            entity.Topic = null;
            DbContext.Comments.Update(entity);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Comment>> GetAllWithDetailsAsync()
        {
            return await DbContext.Comments
                .Include(c => c.Author)
                .Include(c => c.Topic)
                .ThenInclude(t => t.Community)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Comment> GetByIdWithDetailsAsync(int id)
        {
            return await DbContext.Comments
                .Include(c => c.Author)
                .Include(c => c.Topic)
                .ThenInclude(t => t.Community)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
