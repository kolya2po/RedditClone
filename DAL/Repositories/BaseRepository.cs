namespace Data.Repositories
{
    /// <summary>
    /// Base repository.
    /// </summary>
    public class BaseRepository
    {
        /// <summary>
        /// Field of ForumDbContext type.
        /// </summary>
        protected readonly ForumDbContext DbContext;

        /// <summary>
        /// Initialize DbContext field.
        /// </summary>
        /// <param name="dbContext">Instance of the ForumDbContext class.</param>
        protected BaseRepository(ForumDbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
