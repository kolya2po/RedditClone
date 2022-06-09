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
    public class RuleRepositoryTests
    {
        [Test]
        public async Task GetAllAsync_ReturnsAllComments()
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var ruleRepository = new RuleRepository(dbContext);
            var expectedRules = UnitTestHelper.GetTestRules();

            // Act
            var actualRules = await ruleRepository.GetAllAsync();

            // Assert
            actualRules.Should().BeEquivalentTo(expectedRules);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task GetByIdAsync_ReturnsCorrectComment(int id)
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var ruleRepository = new RuleRepository(dbContext);
            var expectedRule = UnitTestHelper.GetTestRules()
                .FirstOrDefault(c => c.Id == id);

            // Act
            var actualRule = await ruleRepository.GetByIdAsync(id);

            // Assert
            actualRule.Should().BeEquivalentTo(expectedRule);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [Test]
        public async Task AddAsync_AddsValueToDatabase()
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var ruleRepository = new RuleRepository(dbContext);

            var newRule = new Rule
            {
                Id = 5,
                CommunityId = 1,
                Title = "Rule 5",
                RuleText = "Never break rule 5"
            };

            // Act
            await ruleRepository.AddAsync(newRule);
            await dbContext.SaveChangesAsync();

            // Assert
            dbContext.Rules.Count().Should().Be(5);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task Delete_DeletesEntity(int id)
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var ruleRepository = new RuleRepository(dbContext);
            var comment = await ruleRepository.GetByIdAsync(id);

            // Act
            ruleRepository.Delete(comment);
            await dbContext.SaveChangesAsync();

            // Assert
            dbContext.Rules.Count().Should().Be(3);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task DeleteByIdAsync_DeletesEntityById(int id)
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var ruleRepository = new RuleRepository(dbContext);

            // Act
            await ruleRepository.DeleteByIdAsync(id);
            await dbContext.SaveChangesAsync();

            // Assert
            dbContext.Rules.Count().Should().Be(3);
            await dbContext.Database.EnsureDeletedAsync();
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task Update_UpdatesEntity(int id)
        {
            // Arrange
            await using var dbContext = new ForumDbContext(UnitTestHelper.GetDbContextOptions());
            var ruleRepository = new RuleRepository(dbContext);

            var oldRule = UnitTestHelper.GetTestRules()
                .FirstOrDefault(c => c.Id == id);
            var newRule = new Rule
            {
                Id = id,
                CommunityId = 1,
                Title = $"New Rule {id}",
                RuleText = $"New Never break rule {id}"
            };

            // Act
            ruleRepository.Update(newRule);
            await dbContext.SaveChangesAsync();

            // Assert
            newRule.Should().NotBeEquivalentTo(oldRule);
            await dbContext.Database.EnsureDeletedAsync();
        }
    }
}
