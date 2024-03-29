﻿namespace Data.Repositories
{
    /// <summary>
    /// Base repository. Contains shared field.
    /// </summary>
    public class BaseRepository
    {
        /// <summary>
        /// Field of ForumDbContext type.
        /// </summary>
        protected readonly ForumDbContext DbContext;

        /// <summary>
        /// Initializes a new instance of the class. Also initializes class's field.
        /// </summary>
        /// <param name="dbContext">Instance of the ForumDbContext class.</param>
        protected BaseRepository(ForumDbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
