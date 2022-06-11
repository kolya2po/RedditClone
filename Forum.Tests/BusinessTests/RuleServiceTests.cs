using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.MapModels;
using Business.Services;
using Business.Validation;
using Data.Interfaces;
using Data.Models;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Forum.Tests.BusinessTests
{
    [TestFixture]
    public class RuleServiceTests
    {
        [Test]
        public async Task GetAllByCommunityIdAsync_ReturnsAllRulesThatBelongsToCommunity()
        {
            // Arrange
            var expected = GetTestRuleModels;
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var community = new Community
            {
                Id = 1,
                Rules = UnitTestHelper.GetTestRules()
            };

            mockUnitOfWork.Setup(u => u.CommunityRepository.GetByIdWithDetailsAsync(1))
                .ReturnsAsync(community);

            var ruleService = new RuleService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());

            // Act
            var actual = await ruleService.GetAllByCommunityIdAsync(1);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetAllByCommunityIdAsync_ThrowsForumExceptionIfCommunityDoesntExist()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork.Setup(u => u.CommunityRepository.GetByIdWithDetailsAsync(1))
                .ReturnsAsync((Community)null);

            var ruleService = new RuleService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());

            // Act
            Func<Task> call = async () => await ruleService.GetAllByCommunityIdAsync(10);

            // Assert
            await call.Should().ThrowAsync<ForumException>();
        }

        [Test]
        public async Task AddAsync_AddsNewRule()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.RuleRepository.AddAsync(It.IsAny<Rule>()));

            var ruleService = new RuleService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());
            var rule = GetTestRuleModels.First();

            // Act
            await ruleService.AddAsync(rule);

            // Assert
            mockUnitOfWork.Verify(u => u.RuleRepository
                .AddAsync(It.Is<Rule>(r =>
                    r.Title == rule.Title && r.CommunityId == rule.CommunityId)), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_UpdatesRule()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.RuleRepository.Update(It.IsAny<Rule>()));
            var ruleService = new RuleService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());

            var rule = GetTestRuleModels.First();

            // Act
            await ruleService.UpdateAsync(rule);

            // Assert
            mockUnitOfWork.Verify(u => u.RuleRepository.Update(It.Is<Rule>(r =>
            r.Title == rule.Title && r.CommunityId == rule.CommunityId)), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_RemovesRuleFromDb()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.RuleRepository.DeleteByIdAsync(1));
            var ruleService = new RuleService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());

            // Act
            await ruleService.DeleteAsync(1);

            // Assert
            mockUnitOfWork.Verify(u => u.RuleRepository.DeleteByIdAsync(1), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        private static IEnumerable<RuleModel> GetTestRuleModels =>
            new []
            {
                new RuleModel
                {
                    Id = 1,
                    CommunityId = 1,
                    Title = "Rule 1",
                    RuleText = "Never break rule 1"
                },
                new RuleModel()
                {
                    Id = 2,
                    CommunityId = 1,
                    Title = "Rule 2",
                    RuleText = "Never break rule 2"
                },
                new RuleModel()
                {
                    Id = 3,
                    CommunityId = 1,
                    Title = "Rule 3",
                    RuleText = "Never break rule 3"
                },
                new RuleModel()
                {
                    Id = 4,
                    CommunityId = 1,
                    Title = "Rule 4",
                    RuleText = "Never break rule 4"
                }
            };
    }
}
