using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.ModelsDbConfiguration
{
    public class TopicConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.HasOne(t => t.Author)
                .WithMany(a => a.Posts)
                .HasForeignKey(t => t.AuthorId);

            builder.HasOne(t => t.Community)
                .WithMany(c => c.Posts)
                .HasForeignKey(t => t.CommunityId);

            builder.HasMany(t => t.Comments)
                .WithOne(c => c.Topic)
                .HasForeignKey(c => c.TopicId);
        }
    }
}
