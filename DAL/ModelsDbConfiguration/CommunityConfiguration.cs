using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.ModelsDbConfiguration
{
    public class CommunityConfiguration : IEntityTypeConfiguration<Community>
    {
        public void Configure(EntityTypeBuilder<Community> builder)
        {
            builder.HasMany(c => c.Rules)
                .WithOne(r => r.Community)
                .HasForeignKey(r => r.CommunityId);

            builder.HasOne(c => c.Creator)
                .WithOne(u => u.CreatedCommunity)
                .HasForeignKey<User>(u => u.CreatedCommunityId);

            builder.HasMany(c => c.Moderators)
                .WithOne(u => u.ModeratedCommunity)
                .HasForeignKey(u => u.ModeratedCommunityId);
        }
    }
}
