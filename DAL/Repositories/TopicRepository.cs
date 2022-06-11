﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class TopicRepository : BaseRepository, ITopicRepository
    {
        /// <inheritdoc />
        public TopicRepository(ForumDbContext dbContext) : base(dbContext)
        {
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Topic>> GetAllAsync()
        {
            return await DbContext.Topics.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Topic> GetByIdAsync(int id)
        {
            return await DbContext.Topics.FirstOrDefaultAsync(t => t.Id == id);
        }

        /// <inheritdoc />
        public async Task AddAsync(Topic entity)
        {
            await DbContext.Topics.AddAsync(entity);
        }

        /// <inheritdoc />
        public void Delete(Topic entity)
        {
            DbContext.Topics.Remove(entity);
        }

        /// <inheritdoc />
        public async Task DeleteByIdAsync(int id)
        {
            var topic = await DbContext.Topics.FirstOrDefaultAsync(t => t.Id == id);

            if (topic != null)
            {
                DbContext.Topics.Remove(topic);
            }
        }

        /// <inheritdoc />
        public void Update(Topic entity)
        {
            DbContext.Topics.Update(entity);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Topic>> GetAllWithDetailsAsync()
        {
            return await DbContext.Topics
                .Include(t => t.Comments)
                .ThenInclude(c => c.Author)
                .Include(t => t.Author)
                .Include(t => t.Community)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Topic> GetByIdWithDetailsAsync(int id)
        {
            return await DbContext.Topics
                .Include(t => t.Comments)
                .ThenInclude(c => c.Author)
                .Include(t => t.Author)
                .Include(t => t.Community)
                .ThenInclude(c => c.Members)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
