using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Data.Repositories;
using FluentAssertions;
using NUnit.Framework;

namespace Forum.Tests.DataTests
{
    [TestFixture]
    public class CommentRepositoryTests
    {
        [Test]
        public async Task GetAllAsync_ReturnsAllComments()
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var commentRepository = new CommentRepository(dbContext);
            var expectedComments = UnitTestHelper.GetTestComments();

            // Act
            var actualComments = await commentRepository.GetAllAsync();

            // Assert
            actualComments.Should().BeEquivalentTo(expectedComments);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task GetByIdAsync_ReturnsCorrectComment(int id)
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var commentRepository = new CommentRepository(dbContext);
            var expectedComment = UnitTestHelper.GetTestComments()
                .FirstOrDefault(c => c.Id == id);

            // Act
            var actualComment = await commentRepository.GetByIdAsync(id);

            // Assert
            actualComment.Should().BeEquivalentTo(expectedComment);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [Test]
        public async Task AddAsync_AddsValueToDatabase()
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var commentRepository = new CommentRepository(dbContext);

            var newComment = new Comment
            {
                Id = 5,
                UserId = 2
            };

            // Act
            await commentRepository.AddAsync(newComment);
            await dbContext.SaveChangesAsync();

            // Assert
            dbContext.Comments.Count().Should().Be(5);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task Delete_DeletesEntity(int id)
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var commentRepository = new CommentRepository(dbContext);
            var comment = await commentRepository.GetByIdAsync(id);

            // Act
            commentRepository.Delete(comment);
            await dbContext.SaveChangesAsync();

            // Assert
            dbContext.Comments.Count().Should().Be(3);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task DeleteByIdAsync_DeletesEntityById(int id)
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var commentRepository = new CommentRepository(dbContext);

            // Act
            await commentRepository.DeleteByIdAsync(id);
            await dbContext.SaveChangesAsync();

            // Assert
            dbContext.Comments.Count().Should().Be(3);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task Update_UpdatesEntity(int id)
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var commentRepository = new CommentRepository(dbContext);

            var oldComment = UnitTestHelper.GetTestComments()
                .FirstOrDefault(c => c.Id == id);
            var newComment = new Comment
            {
                Id = id,
                Rating = 20,
                Text = $"Comment{id} New Text",
                UserId = 1,
                TopicId = 1
            };

            // Act
            commentRepository.Update(newComment);
            await dbContext.SaveChangesAsync();

            // Assert
            newComment.Should().NotBeEquivalentTo(oldComment);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [Test]
        public async Task GetAllWithDetailsAsync_ReturnsWithIncludedEntities()
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var commentRepository = new CommentRepository(dbContext);
            var expectedComments = UnitTestHelper.GetTestComments();

            // Act
            var comments = await commentRepository.GetAllWithDetailsAsync()
                .ContinueWith(c => c.Result.ToList());
            // Because of multiple enumerations

            // Assert
            comments.Should().BeEquivalentTo(expectedComments, opt =>
            {
                return opt.Excluding(c => c.Author)
                    .Excluding(c => c.Replies)
                    .Excluding(c => c.Topic);
            });
            comments.Select(c => c.Author).Should().NotBeNull();
            comments.Select(c => c.Topic).Should().NotBeNull();
            comments.Select(c => c.Topic.Community).Should().NotBeNull();
            comments.Select(c => c.Replies).Should().NotBeNull();
            await dbContext.Database.EnsureDeletedAsync();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task GetByIdWithDetailsAsync_ReturnsWithIncludedEntities(int id)
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var commentRepository = new CommentRepository(dbContext);
            var expectedComment = UnitTestHelper.GetTestComments()
                .FirstOrDefault(c => c.Id == id);

            // Act
            var comment = await commentRepository.GetByIdWithDetailsAsync(id);

            // Assert
            comment.Should().BeEquivalentTo(expectedComment, opt =>
            {
                return opt.Excluding(c => c.Author)
                    .Excluding(c => c.Replies)
                    .Excluding(c => c.Topic);
            });
            comment.Author.Should().NotBeNull();
            comment.Topic.Should().NotBeNull();
            comment.Topic.Community.Should().NotBeNull();
            comment.Replies.Should().NotBeNull();
            await dbContext.Database.EnsureDeletedAsync();
        }
    }
}
