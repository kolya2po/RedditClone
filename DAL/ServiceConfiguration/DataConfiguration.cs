using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data.ServiceConfiguration
{
    public static class DataConfiguration
    {
        public static IServiceCollection AddData(this IServiceCollection collection, IConfiguration configuration)
        {
            collection.AddDbContext<ForumDbContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("Default")));

            collection.AddTransient<IUnitOfWork, UnitOfWork>();

            return collection;
        }
    }
}
