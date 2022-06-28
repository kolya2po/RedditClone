using Business.MapModels;
using Business.Services;
using Data.Interfaces;
using Data.Models;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
            var communityService = new CommunityService(mockUnitOfWork.Object,
                null, UnitTestHelper.CreateMapperFromProfile());

            // Act
            var actual = (await communityService.GetAllAsync()).ToList();

            // Assert
            actual.Should().BeEquivalentTo(GetTestCommunityModels, config =>
                config.Excluding(c => c.CreationDate)
                      .Excluding(c => c.PostModels)
                      .Excluding(c => c.ModeratorModels));

            actual.SelectMany(c => c.PostModels)
                .Should().BeEquivalentTo(GetTestCommunityModels.SelectMany(c => c.PostModels), config =>
                    config.Excluding(p => p.PostingDate)
                          .Excluding(p => p.CommentModels));

            actual.SelectMany(c => c.ModeratorModels)
                .Should().BeEquivalentTo(GetTestCommunityModels.SelectMany(c => c.ModeratorModels), config =>
                    config.Excluding(c => c.BirthDate)
                          .Excluding(c => c.CommunitiesIds)
                          .Excluding(c => c.PostModels)
                          .Excluding(c => c.CommentModels));
        }

        [TestCase(1)]
        public async Task GetByIdAsync_ReturnsCommunity(int id)
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.CommunityRepository.GetByIdWithDetailsAsync(id))
                .ReturnsAsync(GetTestCommunities.First());
            var communityService = new CommunityService(mockUnitOfWork.Object,
                null, UnitTestHelper.CreateMapperFromProfile());

            // Act
            var actual = await communityService.GetByIdAsync(id);

            // Assert
            actual.Should().BeEquivalentTo(GetTestCommunityModels.First(), config =>
                config.Excluding(c => c.CreationDate)
                    .Excluding(c => c.PostModels)
                    .Excluding(c => c.ModeratorModels));

            actual.PostModels
                .Should().BeEquivalentTo(GetTestCommunityModels.First().PostModels, config =>
                    config.Excluding(p => p.PostingDate)
                        .Excluding(p => p.CommentModels));

            actual.ModeratorModels
                .Should().BeEquivalentTo(GetTestCommunityModels.First().ModeratorModels, config =>
                    config.Excluding(c => c.BirthDate)
                        .Excluding(c => c.CommunitiesIds)
                        .Excluding(c => c.PostModels)
                        .Excluding(c => c.CommentModels));
        }

        [Test]
        public async Task UpdateAsync_UpdatesCommunity()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.CommunityRepository.Update(It.IsAny<Community>()));
            var communityService = new CommunityService(mockUnitOfWork.Object,
                null, UnitTestHelper.CreateMapperFromProfile());
            var newCommunity = GetTestCommunityModels.First();

            // Act
            await communityService.UpdateAsync(newCommunity);

            // Assert
            mockUnitOfWork.Verify(u => u.CommunityRepository
                .Update(It.Is<Community>(c => c.Id == newCommunity.Id)), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_DeletesCommunity()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockUserManager = UnitTestHelper.GetMockUserManager();
            mockUserManager.Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            mockUserManager.Setup(u => u.RemoveClaimAsync(It.IsAny<User>(), It.IsAny<Claim>()));

            mockUnitOfWork.Setup(u => u.CommunityRepository.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(GetTestCommunities.First());
            mockUnitOfWork.Setup(u => u.CommunityRepository.DeleteByIdAsync(It.IsAny<int>()));
            var communityService = new CommunityService(mockUnitOfWork.Object,
                mockUserManager.Object, UnitTestHelper.CreateMapperFromProfile());

            // Act
            await communityService.DeleteAsync(1);

            // Assert
            mockUnitOfWork.Verify(u => u.CommunityRepository
                .DeleteByIdAsync(It.IsAny<int>()), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllUsers_ReturnsAllCommunityUsers()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.CommunityRepository
                    .GetByIdWithDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(GetTestCommunities.First());
            var communityService = new CommunityService(mockUnitOfWork.Object,
                null, UnitTestHelper.CreateMapperFromProfile());
            var expected = new List<UserModel> {new UserModel {Id = 1}, new UserModel {Id = 2}};

            // Act
            var actual = await communityService.GetAllUsersAsync(1);

            // Assert
            actual.Should().BeEquivalentTo(expected, config =>
                config.Excluding(c => c.PostModels)
                      .Excluding(c => c.BirthDate)
                      .Excluding(c => c.CommentModels)
                      .Excluding(c => c.CommunitiesIds));
        }

        [Test]
        public async Task AddModeratorAsync_AddsModeratorToTheCommunity()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockUserManager = UnitTestHelper.GetMockUserManager();
            mockUserManager.Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());
            mockUserManager.Setup(u => u
                .AddClaimAsync(It.IsAny<User>(), It.IsAny<Claim>()));
            var communityService = new CommunityService(mockUnitOfWork.Object,
                mockUserManager.Object, UnitTestHelper.CreateMapperFromProfile());

            // Act
            await communityService.AddModeratorAsync(1, 1);

            // Assert
            mockUserManager.Verify(u => u.AddClaimAsync(It.IsAny<User>(), It.IsAny<Claim>()), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task RemoveModeratorAsync_RemovesModerator()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockUserManager = UnitTestHelper.GetMockUserManager();
            mockUserManager.Setup(u => u.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new User {ModeratedCommunityId = 1});
            mockUserManager.Setup(u => u
                .RemoveClaimAsync(It.IsAny<User>(), It.IsAny<Claim>()));
            var communityService = new CommunityService(mockUnitOfWork.Object,
                mockUserManager.Object, UnitTestHelper.CreateMapperFromProfile());

            // Act
            await communityService.RemoveModeratorAsync(1, 1);

            // Assert
            mockUserManager.Verify(u => u.RemoveClaimAsync(It.IsAny<User>(), It.IsAny<Claim>()), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task JoinCommunityAsync_AddUserToTheCommunityMembers()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.UserCommunityRepository.GetAllAsync())
                .ReturnsAsync(new List<UserCommunity>());
            mockUnitOfWork.Setup(u => u.UserCommunityRepository.AddAsync(It.IsAny<UserCommunity>()));

            var communityService = new CommunityService(mockUnitOfWork.Object,
                null, UnitTestHelper.CreateMapperFromProfile());

            // Act
            await communityService.JoinCommunityAsync(1, 1);

            // Assert
            mockUnitOfWork.Verify(u => u.UserCommunityRepository.AddAsync(It.IsAny<UserCommunity>()), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [TestCase(1, 1)]
        public async Task LeaveCommunityAsync_RemovesUserFromTheCommuntyMember(int userId, int communityId)
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.UserCommunityRepository.GetAllAsync())
                .ReturnsAsync(new List<UserCommunity> {new UserCommunity {UserId = userId, CommunityId = communityId}});
            mockUnitOfWork.Setup(u => u.UserCommunityRepository.Delete(It.IsAny<UserCommunity>()));

            var communityService = new CommunityService(mockUnitOfWork.Object,
                null, UnitTestHelper.CreateMapperFromProfile());

            // Act
            await communityService.LeaveCommunityAsync(userId, communityId);

            // Assert
            mockUnitOfWork.Verify(u => u.UserCommunityRepository.Delete(It.IsAny<UserCommunity>()), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        private static IEnumerable<Community> GetTestCommunities =>
            new[]
            {
                new Community
                {
                    Id = 1,
                    Title = "Community 1",
                    CreatorId = 1,
                    Creator = new User { Id = 1, CreatedCommunityId = 1, UserName = "User 1"},
                    Members = new []
                    {
                        new UserCommunity { Id = 1, UserId = 1, CommunityId = 1,
                            User = new User { Id = 1 } },
                        new UserCommunity { Id = 2, UserId = 2, CommunityId = 1,
                            User = new User {Id = 2} }
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
                },
                new Community
                {
                    Id = 2,
                    Title = "Community 2",
                    CreatorId = 2,
                    Creator = new User { Id = 2, CreatedCommunityId = 2, UserName = "User 2" },
                    Members = new []
                    {
                        new UserCommunity { Id = 3, UserId = 1, CommunityId = 2 },
                        new UserCommunity { Id = 4, UserId = 2, CommunityId = 2 },
                    },
                    Moderators = new []
                    {
                        new User { Id = 3, ModeratedCommunityId = 2 },
                        new User { Id = 4, ModeratedCommunityId = 2 },
                    },
                    Rules = new []
                    {
                        new Rule { Id = 3, CommunityId = 2, Title = "Rule 1" },
                        new Rule { Id = 4, CommunityId = 2, Title = "Rule 2" }
                    },
                    Posts = new []
                    {
                        new Topic { Id = 3, CommunityId = 2, AuthorId = 1, Title = "Topic 3"},
                        new Topic { Id = 4, CommunityId = 2, AuthorId = 2, Title = "Topic 4"}
                    }
                }
            };

        private static IEnumerable<CommunityModel> GetTestCommunityModels =>
            new[]
            {
                new CommunityModel
                {
                    Id = 1,
                    Title = "Community 1",
                    CreatorId = 1,
                    MembersCount = 2,
                    ModeratorModels = new []
                    {
                        new UserModel { Id = 1, CreatedCommunityId = 1, ModeratedCommunityId = 1 },
                        new UserModel { Id = 2, ModeratedCommunityId = 1 },
                    },
                    PostModels = new []
                    {
                        new TopicModel { Id = 1, CommunityId = 1, AuthorId = 1, Title = "Topic 1"},
                        new TopicModel { Id = 2, CommunityId = 1, AuthorId = 2, Title = "Topic 2"}
                    },
                    RuleModels = new []
                    {
                        new RuleModel { Id = 1, CommunityId = 1, Title = "Rule 1" },
                        new RuleModel { Id = 2, CommunityId = 1, Title = "Rule 2" }
                    }
                },
                new CommunityModel
                {
                    Id = 2,
                    Title = "Community 2",
                    CreatorId = 2,
                    MembersCount = 2,
                    ModeratorModels = new []
                    {
                        new UserModel { Id = 3, ModeratedCommunityId = 2 },
                        new UserModel { Id = 4, ModeratedCommunityId = 2 },
                    },
                    PostModels = new []
                    {
                        new TopicModel { Id = 3, CommunityId = 2, AuthorId = 1, Title = "Topic 3"},
                        new TopicModel { Id = 4, CommunityId = 2, AuthorId = 2, Title = "Topic 4"}
                    },
                    RuleModels = new []
                    {
                        new RuleModel { Id = 3, CommunityId = 2, Title = "Rule 1" },
                        new RuleModel { Id = 4, CommunityId = 2, Title = "Rule 2" }
                    }
                }
            };
    }
}
