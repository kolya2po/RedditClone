using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data.ServiceConfiguration
{
    /// <summary>
    /// Contains extension method for configuring DI container.
    /// </summary>
    public static class DataConfiguration
    {
        /// <summary>
        /// Registers ForumDbContext and UnitOfWork.
        /// </summary>
        /// <param name="collection">Instance that implements IServiceCollection interface.</param>
        /// <param name="configuration">Instance that implements IConfiguration interface.</param>
        /// <returns>Configured instance that implements IServiceCollection interface. </returns>
        public static IServiceCollection AddData(this IServiceCollection collection, IConfiguration configuration)
        {
            collection.AddDbContext<ForumDbContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("Default")));

            collection.AddTransient<IUnitOfWork, UnitOfWork>();

            return collection;
        }
    }
}
