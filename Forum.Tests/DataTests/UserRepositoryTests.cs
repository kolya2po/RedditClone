using System.Collections.Generic;
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
    public class UserRepositoryTests
    {
        [Test]
        public async Task GetAllWithDetailsAsync_ReturnsWithIncludedEntities()
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var userRepository = new UserRepository(dbContext);

            // Act
            var actual = (await userRepository.GetAllWithDetailsAsync()).ToList();

            // Assert
            actual.Should().BeEquivalentTo(GetExpectedUsers, config =>
            {
                return config.Excluding(u => u.Posts)
                    .Excluding(u => u.Communities)
                    .Excluding(u => u.Comments)
                    .Excluding(u => u.ConcurrencyStamp);
            });
            actual.SelectMany(u => u.Posts)
                .Should()
                .BeEquivalentTo(GetExpectedUsers.SelectMany(u =>
                    u.Posts), config =>
                {
                    return config.Excluding(t => t.Comments)
                        .Excluding(t => t.Author);
                });
            actual.SelectMany(u => u.Comments).Should()
                .BeEquivalentTo(GetExpectedUsers.SelectMany(u =>
                    u.Comments), config =>
                {
                    return config.Excluding(c => c.Author)
                        .Excluding(c => c.Topic);
                });

            await dbContext.Database.EnsureDeletedAsync();
        }

        [TestCase(1)]
        public async Task GetByIdWithDetailsAsync_ReturnsWithIncludedEntities(int id)
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var userRepository = new UserRepository(dbContext);
            var expected = GetExpectedUsers.First(u => u.Id == id);

            // Act
            var actual = await userRepository.GetByIdWithDetailsAsync(id);

            // Assert
            actual.Should().BeEquivalentTo(expected, config =>
            {
                return config.Excluding(u => u.ModeratedCommunity)
                    .Excluding(u => u.Communities)
                    .Excluding(u => u.Posts)
                    .Excluding(u => u.Comments)
                    .Excluding(u => u.ConcurrencyStamp);
            });
            actual.Posts.Should().BeEquivalentTo(expected.Posts, config =>
            {
                return config.Excluding(t => t.Comments)
                    .Excluding(t => t.Author)
                    .Excluding(t => t.Community);
            });
            actual.Comments.Should().BeEquivalentTo(expected.Comments, config =>
            {
                return config.Excluding(c => c.Author)
                    .Excluding(c => c.Topic);
            });

            await dbContext.Database.EnsureDeletedAsync();
        }

        private static IEnumerable<User> GetExpectedUsers =>
            new[]
            {
                new User
                {
                    Id = 1,
                    UserName = "Name 1",
                    ModeratedCommunityId = 1,
                    CreatedCommunityId = 1,
                    Posts = UnitTestHelper.GetTestTopics().Where(t => t.AuthorId == 1),
                    Comments = UnitTestHelper.GetTestComments().Where(c => c.AuthorId == 1),
                    Communities = UnitTestHelper.GetTestUserCommunities().Where(uc => uc.UserId == 1)
                },
                new User
                {
                    Id = 2,
                    UserName = "Name 2",
                    CreatedCommunityId = 2,
                    Posts = UnitTestHelper.GetTestTopics().Where(t => t.AuthorId == 2),
                    Comments = UnitTestHelper.GetTestComments().Where(c => c.AuthorId == 2),
                    Communities = UnitTestHelper.GetTestUserCommunities().Where(uc => uc.UserId == 2)
                },
                new User
                {
                    Id = 3,
                    UserName = "Name 3",
                    ModeratedCommunityId = 2,
                    CreatedCommunityId = 3,
                    Posts = UnitTestHelper.GetTestTopics().Where(t => t.AuthorId == 3),
                    Comments = UnitTestHelper.GetTestComments().Where(c => c.AuthorId == 3),
                    Communities = UnitTestHelper.GetTestUserCommunities().Where(uc => uc.UserId == 3)
                },
                new User
                {
                    Id = 4,
                    UserName = "Name 4",
                    Posts = UnitTestHelper.GetTestTopics().Where(t => t.AuthorId == 4),
                    Comments = UnitTestHelper.GetTestComments().Where(c => c.AuthorId == 4),
                    Communities = UnitTestHelper.GetTestUserCommunities().Where(uc => uc.UserId == 4)
                }
            };
    }
}
