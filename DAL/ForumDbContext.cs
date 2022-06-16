using System.Collections.Generic;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    /// <summary>
    /// Extends IdentityDbContext class by adding few more DbSets.
    /// </summary>
    public class ForumDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        /// <summary>
        /// Initializes a new instance of the ForumDbContext.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of Comments.
        /// </summary>
        public DbSet<Comment> Comments { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of Communities.
        /// </summary>
        public DbSet<Community> Communities { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of Rules.
        /// </summary>
        public DbSet<Rule> Rules { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of Topics.
        /// </summary>
        public DbSet<Topic> Topics { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of UserCommunities.
        /// </summary>
        public DbSet<UserCommunity> UserCommunities { get; set; }

        /// <summary>
        /// Applies configurations for entities.
        /// Configures User and IdentityRole entities.
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ForumDbContext).Assembly);

            builder.Entity<User>()
                .HasIndex(i => i.Email)
                .IsUnique();

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
                    }
                });
        }
    }
}
