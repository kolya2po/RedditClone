using System.Security.Claims;
using System.Text;
using Business.Interfaces;
using Business.Services;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Business.ServiceConfiguration
{
    /// <summary>
    /// Contains extension methods for configuring DI container.
    /// </summary>
    public static class BusinessConfiguration
    {
        /// <summary>
        /// Registers all services from BLL.
        /// </summary>
        /// <param name="service">Instance that implements IServiceCollection interface.</param>
        /// <returns>Configured instance that implements IServiceCollection interface.</returns>
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

        /// <summary>
        /// Registers identity's services. Adds authentication bases on the JWT token.
        /// Also adds authorization for moderators.
        /// </summary>
        /// <param name="services">Instance that implements IServiceCollection interface.</param>
        /// <param name="configuration">Instance that implements IConfiguration interface.</param>
        /// <returns>Configured instance that implements IServiceCollection interface. </returns>
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<User, IdentityRole<int>>(opt =>
                {
                    opt.SignIn.RequireConfirmedAccount = false;
                    opt.Password.RequiredLength = 5;
                    opt.Password.RequireDigit = false;
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<ForumDbContext>();

            var jwtSettings = configuration.GetSection("Auth");
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["validIssuer"],

                    ValidateAudience = true,
                    ValidAudience = jwtSettings["validAudience"],

                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey = new SymmetricSecurityKey
                    (
                        Encoding.UTF8.GetBytes(jwtSettings["secret"])
                    )
                };
            });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("ModeratorOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Moderator"));
            });

            services.AddScoped<IJwtHandler, JwnHandler>();
            return services;
        }
    }
}
