using System.Collections.Generic;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ForumDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options)
        {
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<UserCommunity> UserCommunities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ForumDbContext).Assembly);

            builder.Entity<IdentityRole<int>>()
                .HasData(new List<IdentityRole<int>>
                {
                    new IdentityRole<int>
                    {
                        Id = 1,
                        Name = "Registered",
                        NormalizedName = "REGISTERED"
                    },
                    new IdentityRole<int>
                    {
                        Id = 2,
                        Name = "Moderator",
                        NormalizedName = "MODERATOR"
                    },
                    new IdentityRole<int>
                    {
                        Id = 3,
                        Name = "Administrator",
                        NormalizedName = "ADMINISTRATOR"
                    }
                });

            builder.Entity<User>().HasData(new User
            {
                Id = 1,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@email.com",
                NormalizedEmail = "ADMIN@EMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<User>().HashPassword(null, "adminPassword")
            });

            builder.Entity<IdentityUserRole<int>>().HasData(new IdentityUserRole<int>
            {
                RoleId = 3,
                UserId = 1
            });
        }
    }
}
