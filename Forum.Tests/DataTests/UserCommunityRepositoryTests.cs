using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Models;
using Data.Repositories;
using FluentAssertions;
using NUnit.Framework;

namespace Forum.Tests.DataTests
{
    // TODO: Have to be done.
    [TestFixture]
    public class UserCommunityRepositoryTests
    {
        [Test]
        public async Task GetAllAsync_ReturnsAllUserCommunities()
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var userCommunityRepository = new UserCommunityRepository(dbContext);
            var expected = UnitTestHelper.GetTestUserCommunities();

            // Act
            var actual = await userCommunityRepository.GetAllAsync();

            // Assert
            actual.Should().BeEquivalentTo(expected);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task GetByIdAsync_ReturnsCorrectUserCommunity(int id)
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var userCommunityRepository = new UserCommunityRepository(dbContext);
            var expected= UnitTestHelper.GetTestUserCommunities()
                .FirstOrDefault(uc => uc.Id == id);

            // Act
            var actual = await userCommunityRepository.GetByIdAsync(id);

            // Assert
            actual.Should().BeEquivalentTo(expected);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [Test]
        public async Task AddAsync_AddsValueToDatabase()
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var userCommunityRepository = new UserCommunityRepository(dbContext);

            var newUserCommunity= new UserCommunity()
            {
                CommunityId = 3,
                UserId = 1
            };

            // Act
            await userCommunityRepository.AddAsync(newUserCommunity);
            await dbContext.SaveChangesAsync();

            // Assert
            dbContext.UserCommunities.Count().Should().Be(5);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task Delete_DeletesEntity(int id)
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var userCommunityRepository = new UserCommunityRepository(dbContext);
            var topic = await userCommunityRepository.GetByIdAsync(id);

            // Act
            userCommunityRepository.Delete(topic);
            await dbContext.SaveChangesAsync();

            // Assert
            dbContext.UserCommunities.Count().Should().Be(3);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task DeleteByIdAsync_DeletesEntityById(int id)
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var userCommunityRepository = new UserCommunityRepository(dbContext);

            // Act
            await userCommunityRepository.DeleteByIdAsync(id);
            await dbContext.SaveChangesAsync();

            // Assert
            dbContext.UserCommunities.Count().Should().Be(3);
            await dbContext.Database.EnsureDeletedAsync();
        }

        //[TestCase(1)]
        //[TestCase(2)]
        //public async Task Update_UpdatesEntity(int id)
        //{
        //    // Arrange
        //    await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
        //    var userCommunityRepository = new UserCommunityRepository(dbContext);

        //    var oldUserCommunity = UnitTestHelper.GetTestUserCommunities()
        //        .FirstOrDefault(uc => uc.Id == id);

        //    // Joining user to another community.
        //    var newUserCommunity = new UserCommunity
        //    {
        //        Id = id,
        //        CommunityId = id + 1,
        //        AuthorId = id
        //    };

        //    // Act
        //    userCommunityRepository.Update(newUserCommunity);
        //    await dbContext.SaveChangesAsync();

        //    // Assert
        //    newUserCommunity.Should().NotBeEquivalentTo(oldUserCommunity, opt =>
        //    {
        //        return opt.Excluding(uc => uc.AuthorId);
        //    });
        //    await dbContext.Database.EnsureDeletedAsync();
        //}

        [Test]
        public async Task GetAllWithDetailsAsync_ReturnsWithIncludedEntities()
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var userCommunityRepository = new UserCommunityRepository(dbContext);
            var expectedUserCommunities = UnitTestHelper.GetTestUserCommunities();

            // Act
            var userCommunities = await userCommunityRepository.GetAllWithDetailsAsync()
                .ContinueWith(uc => uc.Result.ToList());
            // Because of multiple enumerations

            // Assert
            userCommunities.Should().BeEquivalentTo(expectedUserCommunities, opt =>
            {
                return opt.Excluding(uc => uc.Community)
                    .Excluding(uc => uc.User);
            });

            userCommunities.Select(uc => uc.Community).Should().NotBeNull();
            userCommunities.Select(uc => uc.User).Should().NotBeNull();

            await dbContext.Database.EnsureDeletedAsync();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task GetByIdWithDetailsAsync_ReturnsWithIncludedEntities(int id)
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var userCommunityRepository = new UserCommunityRepository(dbContext);
            var expectedUserCommunity = UnitTestHelper.GetTestUserCommunities()
                .FirstOrDefault(uc => uc.Id == id);

            // Act
            var userCommunity = await userCommunityRepository.GetByIdWithDetailsAsync(id);

            // Assert
            userCommunity.Should().BeEquivalentTo(expectedUserCommunity, opt =>
            {
                return opt.Excluding(uc => uc.Community)
                    .Excluding(uc => uc.User);
            });

            userCommunity.Community.Should().NotBeNull();
            userCommunity.User.Should().NotBeNull();
            await dbContext.Database.EnsureDeletedAsync();
        }
    }
}
