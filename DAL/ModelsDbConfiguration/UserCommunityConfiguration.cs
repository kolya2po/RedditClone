using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.ModelsDbConfiguration
{
    public class UserCommunityConfiguration : IEntityTypeConfiguration<UserCommunity>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<UserCommunity> builder)
        {
            builder.HasOne(uc => uc.User)
                .WithMany(u => u.Communities)
                .HasForeignKey(uc => uc.UserId);

            builder.HasOne(uc => uc.Community)
                .WithMany(c => c.Members)
                .HasForeignKey(uc => uc.CommunityId);
        }
    }
}
