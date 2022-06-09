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
    public class TopicRepositoryTests
    {
        [Test]
        public async Task GetAllAsync_ReturnsAllComments()
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var topicRepository = new TopicRepository(dbContext);
            var expectedTopics = UnitTestHelper.GetTestTopics();

            // Act
            var actualTopics = await topicRepository.GetAllAsync();

            // Assert
            actualTopics.Should().BeEquivalentTo(expectedTopics);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task GetByIdAsync_ReturnsCorrectComment(int id)
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var topicRepository = new TopicRepository(dbContext);
            var expectedTopic = UnitTestHelper.GetTestTopics()
                .FirstOrDefault(c => c.Id == id);

            // Act
            var actualComment = await topicRepository.GetByIdAsync(id);

            // Assert
            actualComment.Should().BeEquivalentTo(expectedTopic);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [Test]
        public async Task AddAsync_AddsValueToDatabase()
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var topicRepository = new TopicRepository(dbContext);

            var newTopic = new Topic
            {
                Id = 5,
                UserId = 2
            };

            // Act
            await topicRepository.AddAsync(newTopic);
            await dbContext.SaveChangesAsync();

            // Assert
            dbContext.Topics.Count().Should().Be(5);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task Delete_DeletesEntity(int id)
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var topicRepository = new TopicRepository(dbContext);
            var topic = await topicRepository.GetByIdAsync(id);

            // Act
            topicRepository.Delete(topic);
            await dbContext.SaveChangesAsync();

            // Assert
            dbContext.Topics.Count().Should().Be(3);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task DeleteByIdAsync_DeletesEntityById(int id)
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var topicRepository = new TopicRepository(dbContext);

            // Act
            await topicRepository.DeleteByIdAsync(id);
            await dbContext.SaveChangesAsync();

            // Assert
            dbContext.Topics.Count().Should().Be(3);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task Update_UpdatesEntity(int id)
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var topicRepository = new CommentRepository(dbContext);

            var oldTopic = UnitTestHelper.GetTestTopics()
                .FirstOrDefault(c => c.Id == id);
            var newTopic = new Comment
            {
                Id = id,
                Rating = 20,
                Text = $"Comment{id} New Text",
                UserId = 1,
                TopicId = 1
            };

            // Act
            topicRepository.Update(newTopic);
            await dbContext.SaveChangesAsync();

            // Assert
            newTopic.Should().NotBeEquivalentTo(oldTopic);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [Test]
        public async Task GetAllWithDetailsAsync_ReturnsWithIncludedEntities()
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var topicRepository = new TopicRepository(dbContext);
            var expectedTopics = UnitTestHelper.GetTestTopics();

            // Act
            var topics = await topicRepository.GetAllWithDetailsAsync()
                .ContinueWith(c => c.Result.ToList());
            // Because of multiple enumerations

            // Assert
            topics.Should().BeEquivalentTo(expectedTopics, opt =>
            {
                return opt.Excluding(c => c.Author)
                    .Excluding(c => c.Comments)
                    .Excluding(c => c.Community);
            });
            topics.Select(c => c.Community).Should().NotBeNull();
            topics.Select(c => c.Author).Should().NotBeNull();
            topics.Select(c => c.Comments).Should().NotBeNull();
            await dbContext.Database.EnsureDeletedAsync();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task GetByIdWithDetailsAsync_ReturnsWithIncludedEntities(int id)
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var topicRepository = new TopicRepository(dbContext);
            var expectedTopics = UnitTestHelper.GetTestTopics()
                .FirstOrDefault(c => c.Id == id);

            // Act
            var topic = await topicRepository.GetByIdWithDetailsAsync(id);

            // Assert
            topic.Should().BeEquivalentTo(expectedTopics, opt =>
            {
                return opt.Excluding(c => c.Author)
                    .Excluding(c => c.Comments)
                    .Excluding(c => c.Community);
            });
            topic.Community.Should().NotBeNull();
            topic.Author.Should().NotBeNull();
            topic.Comments.Should().NotBeNull();
            await dbContext.Database.EnsureDeletedAsync();
        }
    }
}
