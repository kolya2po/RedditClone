namespace Data.Repositories
{
    public class BaseRepository
    {
        protected readonly ForumDbContext DbContext;

        protected BaseRepository(ForumDbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
