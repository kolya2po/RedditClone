using Business.Interfaces;
using Business.Services;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Business.ServiceConfiguration
{
    public static class BusinessConfiguration
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection service)
        {
            service.AddTransient<ICommentService, CommentService>();
            service.AddTransient<ICommunityService, CommunityService>();
            service.AddTransient<IRuleService, RuleService>();
            service.AddTransient<ITopicService, TopicService>();
            service.AddTransient<IUserService, UserService>();

            service.AddAutoMapper(opt => opt.AddMaps(typeof(AutoMapperBllProfile).Assembly));
            return service;
        }

        public static IServiceCollection AddIdentityServices(this IServiceCollection service)
        {
            service.AddIdentity<User, IdentityRole<int>>(opt =>
                {
                    opt.SignIn.RequireConfirmedAccount = false;
                    opt.Password.RequiredLength = 5;
                    opt.Password.RequireDigit = false;
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<ForumDbContext>();
            return service;
        }
    }
}
