using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Services;
using Data.Interfaces;
using Data.Models;
using Moq;
using NUnit.Framework;

namespace Forum.Tests.BusinessTests
{
    [TestFixture]
    public class CommunityServiceTests
    {
        [Test]
        public async Task GetAllAsync_ReturnsAllCommunities()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.CommunityRepository.GetAllWithDetailsAsync())
                .ReturnsAsync(GetTestCommunities);
            var communityService = new CommunityService(mockUnitOfWork.Object, null, UnitTestHelper.CreateMapperFromProfile(),
                null);

            // Act
            var actual = await communityService.GetAllAsync();

            // Assert
        }

        private static IEnumerable<Community> GetTestCommunities =>
            new[]
            {
                new Community
                {
                    Id = 1,
                    Title = "Community 1",
                    CreatorId = 1,
                    Creator = new User { Id = 1, CreatedCommunityId = 1 },
                    Members = new []
                    {
                        new UserCommunity { Id = 1, UserId = 1, CommunityId = 1 },
                        new UserCommunity { Id = 2, UserId = 2, CommunityId = 1 },
                    },
                    Moderators = new []
                    {
                        new User { Id = 1, CreatedCommunityId = 1, ModeratedCommunityId = 1},
                        new User { Id = 2, ModeratedCommunityId = 1 },
                    },
                    Rules = new []
                    {
                        new Rule { Id = 1, CommunityId = 1, Title = "Rule 1" },
                        new Rule { Id = 2, CommunityId = 1, Title = "Rule 2" }
                    },
                    Posts = new []
                    {
                        new Topic { Id = 1, CommunityId = 1, AuthorId = 1, Title = "Topic 1"},
                        new Topic { Id = 2, CommunityId = 1, AuthorId = 2, Title = "Topic 2"}
                    }
                }
            };
    }
}
