using System;
using System.Linq;
using Data;
using Data.Repositories;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using Data.Models;

namespace Forum.Tests.DataTests
{
    [TestFixture]
    public class CommunityRepositoryTests
    {
        [Test]
        public async Task GetAllAsync_ReturnsAllCommunities()
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var communityRepository = new CommunityRepository(dbContext);
            var expected = UnitTestHelper.GetTestCommunities();

            // Act
            var actual = await communityRepository.GetAllAsync();

            // Assert
            actual.Should().BeEquivalentTo(expected);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task GetByIdAsync_ReturnsCorrectCommunity(int id)
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var communityRepository = new CommunityRepository(dbContext);
            var expected = UnitTestHelper.GetTestCommunities()
                .FirstOrDefault(c => c.Id == id);

            // Act
            var actual = await communityRepository.GetByIdAsync(id);

            // Assert
            actual.Should().BeEquivalentTo(expected);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [Test]
        public async Task AddAsync_AddEntityToDatabase()
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var communityRepository = new CommunityRepository(dbContext);
            var newCommunity = new Community
            {
                Id = 4,
                Title = "Community 4",
                About = "About community 4",
                CreationDate = new DateTime(2022, 4, 16)
            };

            // Act
            await communityRepository.AddAsync(newCommunity);
            await dbContext.SaveChangesAsync();

            // Assert
            dbContext.Communities.Count().Should().Be(4);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task Delete_DeletesEntity(int id)
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var communityRepository = new CommunityRepository(dbContext);
            var community = UnitTestHelper.GetTestCommunities()
                .FirstOrDefault(c => c.Id == id);

            // Act
            communityRepository.Delete(community);
            await dbContext.SaveChangesAsync();

            // Assert
            dbContext.Communities.Count().Should().Be(2);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task DeleteByIdAsync_DeletesEntity(int id)
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var communityRepository = new CommunityRepository(dbContext);

            // Act
            await communityRepository.DeleteByIdAsync(id);
            await dbContext.SaveChangesAsync();

            // Assert
            dbContext.Communities.Count().Should().Be(2);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task Update_UpdatesEntity(int id)
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var communityRepository = new CommunityRepository(dbContext);
            var oldCommunity = UnitTestHelper.GetTestCommunities()
                .FirstOrDefault(c => c.Id == id);

            var newCommunity = new Community
            {
                Id = id,
                Title = $"New Community {id}",
                About = $"About community {id}",
                CreationDate = new DateTime(2022, 1, 16)
            };

            // Act
            communityRepository.Update(newCommunity);
            await dbContext.SaveChangesAsync();

            // Assert
            newCommunity.Should().NotBeEquivalentTo(oldCommunity);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [Test]
        public async Task GetAllWithDetailsAsync_ReturnsWithIncludedEntities()
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var communityRepository = new CommunityRepository(dbContext);
            var expectedCommunities = UnitTestHelper.GetTestCommunities();

            // Act
            var communities = await communityRepository.GetAllWithDetailsAsync()
                .ContinueWith(c => c.Result.ToList());

            // Assert
            communities.Should().BeEquivalentTo(expectedCommunities, opt =>
            {
                return opt.Excluding(c => c.Members)
                    .Excluding(c => c.Moderators)
                    .Excluding(c => c.Posts)
                    .Excluding(c => c.Rules)
                    .Excluding(c => c.Creator);
            });
            communities.Select(c => c.Rules).Should().NotBeNull();
            communities.Select(c => c.Posts).Should().NotBeNull();
            communities.SelectMany(c => c.Posts).Select(c => c.Author).Should().NotBeNull();
            communities.SelectMany(c => c.Posts).Select(p => p.Comments).Should().NotBeNull();
            communities.Select(c => c.Members).Should().NotBeNull();
            communities.Select(c => c.Moderators).Should().NotBeNull();
            communities.Select(c => c.Creator).Should().NotBeNull();
            await dbContext.Database.EnsureDeletedAsync();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task GetByIdWithDetailsAsync_ReturnsWithIncludedEntities(int id)
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var communityRepository = new CommunityRepository(dbContext);
            var expectedCommunity = UnitTestHelper.GetTestCommunities()
                .FirstOrDefault(c => c.Id == id);

            // Act
            var community = await communityRepository.GetByIdWithDetailsAsync(id);

            // Assert
            community.Should().BeEquivalentTo(expectedCommunity, opt =>
            {
                return opt.Excluding(c => c.Members)
                    .Excluding(c => c.Moderators)
                    .Excluding(c => c.Posts)
                    .Excluding(c => c.Rules)
                    .Excluding(c => c.Creator);
            });
            community.Rules.Should().NotBeNull();
            community.Posts.Should().NotBeNull();
            community.Posts.Select(p => p.Author).Should().NotBeNull();
            community.Posts.Select(p => p.Comments).Should().NotBeNull();
            community.Members.Should().NotBeNull();
            community.Moderators.Should().NotBeNull();
            await dbContext.Database.EnsureDeletedAsync();
        }
    }
}
