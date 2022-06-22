using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.ModelsDbConfiguration
{
    /// <summary>
    /// Implements IEntityTypeConfiguration interface.
    /// Provides configuration for entities of type Community.
    /// </summary>
    public class CommunityConfiguration : IEntityTypeConfiguration<Community>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Community> builder)
        {
            builder.HasIndex(c => c.Title)
                .IsUnique();

            builder.HasMany(c => c.Rules)
                .WithOne()
                .HasForeignKey(r => r.CommunityId);

            builder.HasOne(c => c.Creator)
                .WithOne()
                .HasForeignKey<User>(u => u.CreatedCommunityId);

            builder.HasMany(c => c.Moderators)
                .WithOne(u => u.ModeratedCommunity)
                .HasForeignKey(u => u.ModeratedCommunityId);
        }
    }
}
