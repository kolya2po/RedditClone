using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.ModelsDbConfiguration
{
    /// <summary>
    /// Implements IEntityTypeConfiguration interface.
    /// Provides configuration for entities of type Comment.
    /// </summary>
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasOne(c => c.Author)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.AuthorId);
        }
    }
}
