using System;
using System.Collections.Generic;
using System.Linq;
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
    public class TopicServiceTests
    {
        [Test]
        public async Task GetAllAsync_ReturnsAllTopics()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.TopicRepository.GetAllWithDetailsAsync())
                .ReturnsAsync(UnitTestHelper.GetTestTopics);
            var expected = GetTestTopicModels();
            var topicService = new TopicService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());

            // Act
            var actual = await topicService.GetAllAsync();

            // Assert
            actual.Should().BeEquivalentTo(expected, opt =>
            {
                return opt.Excluding(c => c.CommentModels)
                    .Excluding(c => c.PostingDate);
            });
        }

        [Test]
        public async Task GetByIdAsync_ReturnsCorrectTopicModel()
        {
            // Arrange
            var topic = GetTestTopic;
            var expected = GetTestTopicModel;
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.TopicRepository.GetByIdWithDetailsAsync(1))
                .ReturnsAsync(topic);
            var topicService = new TopicService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());

            // Act
            var actual = await topicService.GetByIdAsync(1);

            // Assert
            actual.Should().BeEquivalentTo(expected, opt =>
            {
                return opt.Excluding(c => c.CommentModels)
                    .Excluding(c => c.PostingDate);
            });
        }

        [Test]
        public async Task AddAsync_AddsTopicToDatabase()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.TopicRepository.AddAsync(It.IsAny<Topic>()));
            var topicService = new TopicService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());
            var expected = GetTestTopicModels().First();

            // Act
            await topicService.AddAsync(expected);

            // Assert
            mockUnitOfWork.Verify(u => u.TopicRepository.AddAsync(It.Is<Topic>(t => t.Id == expected.Id && t.CommunityId == expected.CommunityId)), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_UpdatesTopicInDatabase()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.TopicRepository.Update(It.IsAny<Topic>()));
            var topicService = new TopicService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());
            var expected = GetTestTopicModels().First();

            // Act
            await topicService.UpdateAsync(expected);

            // Assert
            mockUnitOfWork.Verify(u => u.TopicRepository.Update(It.Is<Topic>(t => t.Id == expected.Id && t.CommunityId == expected.CommunityId)), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_RemovesTopicFromDatabase()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.TopicRepository.DeleteByIdAsync(1));
            var topicService = new TopicService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());

            // Act
            await topicService.DeleteAsync(1);

            // Assert
            mockUnitOfWork.Verify(u => u.TopicRepository.DeleteByIdAsync(1), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task IncreaseRatingAsync_IncreasesTopicsRating()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.TopicRepository.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(GetTestTopic);
            mockUnitOfWork.Setup(u => u.TopicRepository.Update(It.IsAny<Topic>()));
            var topicService = new TopicService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());

            // Act
            await topicService.IncreaseRatingAsync(1);

            // Assert
            mockUnitOfWork.Verify(u => u.TopicRepository
                .Update(It.Is<Topic>(t => t.Id == 1 && t.Rating == GetTestTopic.Rating + 1)), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task IncreaseRatingAsync_ThrowsForumExceptionIfTopicIsNull()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.TopicRepository.GetByIdAsync(It.IsAny<int>()));
            var topicService = new TopicService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());

            // Act
            Func<Task> act = async () => await topicService.IncreaseRatingAsync(1);

            // Assert
            await act.Should().ThrowAsync<ForumException>();
        }

        [Test]
        public async Task DecreaseRatingAsync_DecreasesTopicsRating()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.TopicRepository.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(GetTestTopic);
            mockUnitOfWork.Setup(u => u.TopicRepository.Update(It.IsAny<Topic>()));
            var topicService = new TopicService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());

            // Act
            await topicService.DecreaseRatingAsync(1);

            // Assert
            mockUnitOfWork.Verify(u => u.TopicRepository
                .Update(It.Is<Topic>(t => t.Id == 1 && t.Rating == GetTestTopic.Rating - 1)), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task DecreaseRatingAsync_ThrowsForumExceptionIfTopicIsNull()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.TopicRepository.GetByIdAsync(It.IsAny<int>()));
            var topicService = new TopicService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());

            // Act
            Func<Task> act = async () => await topicService.DecreaseRatingAsync(1);

            // Assert
            await act.Should().ThrowAsync<ForumException>();
        }

        [Test]
        public async Task PinTopicAsync_PinsTopic()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.TopicRepository.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(GetTestTopic);
            mockUnitOfWork.Setup(u => u.TopicRepository.Update(It.IsAny<Topic>()));
            var topicService = new TopicService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());

            // Act
            await topicService.PinTopicAsync(1);

            // Assert
            mockUnitOfWork.Verify(u => u.TopicRepository
                .Update(It.Is<Topic>(t => t.Id == 1 && t.IsPinned)), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task PinTopicAsync_ThrowsForumExceptionIfTopicIsNull()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.TopicRepository.GetByIdAsync(It.IsAny<int>()));
            var topicService = new TopicService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());

            // Act
            Func<Task> act = async () => await topicService.PinTopicAsync(1);

            // Assert
            await act.Should().ThrowAsync<ForumException>();
        }

        [Test]
        public async Task UnpinTopicAsync_UnpinsTopic()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.TopicRepository.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(GetTestTopic);
            mockUnitOfWork.Setup(u => u.TopicRepository.Update(It.IsAny<Topic>()));
            var topicService = new TopicService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());

            // Act
            await topicService.UnpinTopicAsync(1);

            // Assert
            mockUnitOfWork.Verify(u => u.TopicRepository
                .Update(It.Is<Topic>(t => t.Id == 1 && !t.IsPinned)), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task UnpinTopicAsync_ThrowsForumExceptionIfTopicIsNull()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.TopicRepository.GetByIdAsync(It.IsAny<int>()));
            var topicService = new TopicService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());

            // Act
            Func<Task> act = async () => await topicService.UnpinTopicAsync(1);

            // Assert
            await act.Should().ThrowAsync<ForumException>();
        }

        [TestCase(1)]
        public async Task BlockCommentsAsync_BlocksComments(int topicId)
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.TopicRepository.GetByIdAsync(topicId))
                .ReturnsAsync(GetTestTopic);
            var topicService = new TopicService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());
            var expected = GetTestTopicModel;
            expected.CommentsAreBlocked = true;

            // Act
            await topicService.BlockCommentsAsync(topicId);

            // Assert
            mockUnitOfWork.Verify(u => u.TopicRepository.Update(It.Is<Topic>(t => t.Id == expected.Id && t.CommentsAreBlocked == expected.CommentsAreBlocked)), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }


        [Test]
        public async Task BlockCommentsAsync_ThrowsForumExceptionIfTopicIsNull()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.TopicRepository.GetByIdAsync(It.IsAny<int>()));
            var topicService = new TopicService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());

            // Act
            Func<Task> act = async () => await topicService.BlockCommentsAsync(1);

            // Assert
            await act.Should().ThrowAsync<ForumException>();
        }

        private static TopicModel GetTestTopicModel => new TopicModel
        {

            Id = 1,
            AuthorId = 1,
            CommunityId = 1,
            Title = "Topic 1",
            Text = "Text topic 1",
            PostingDate = new DateTime(2022, 1, 22).ToLongDateString(),
            AuthorName = "Author111",
            CommentModels = new []
            {
                new CommentModel { Id = 1, AuthorId = 1, AuthorName = "Author1" },
                new CommentModel { Id = 2, AuthorId = 2, AuthorName = "Author2" },
            },
            CommentsCount = 2,
            CommunityName = "Community111"
        };

        private static Topic GetTestTopic => new Topic
        {
            Id = 1,
            AuthorId = 1,
            CommunityId = 1,
            Title = "Topic 1",
            Text = "Text topic 1",
            PostingDate = new DateTime(2022, 1, 22),
            Comments = new[]
            {
                new Comment { Id = 1, AuthorId = 1, Author = new User
                    { Id = 1, UserName = "Author1"}},
                new Comment { Id = 2, AuthorId = 2, Author = new User
                    { Id = 2, UserName = "Author2"} }
            },
            Author = new User { Id = 1, UserName = "Author111" },
            Community = new Community { Id = 1, Title = "Community111" }
        };

        private static IEnumerable<TopicModel> GetTestTopicModels() =>
            new[]
            {
                new TopicModel
                {
                    Id = 1,
                    AuthorId = 1,
                    CommunityId = 1,
                    Title = "Topic 1",
                    Text = "Text topic 1",
                    PostingDate = new DateTime(2022, 1, 22).ToLongDateString()
                },
                new TopicModel
                {
                    Id = 2,
                    AuthorId = 2,
                    CommunityId = 1,
                    Title = "Topic 2",
                    Text = "Text topic 2",
                    PostingDate = new DateTime(2022, 6, 4).ToLongDateString()
                },
                new TopicModel
                {
                    Id = 3,
                    AuthorId = 3,
                    CommunityId = 2,
                    Title = "Topic 3",
                    Text = "Text topic 3",
                    PostingDate = new DateTime(2022, 2, 16).ToLongDateString()
                },
                new TopicModel
                {
                    Id = 4,
                    AuthorId = 4,
                    CommunityId = 2,
                    Title = "Topic 4",
                    Text = "Text topic 4",
                    PostingDate = new DateTime(2021, 1, 16).ToLongDateString()
                }
            };
    }
}
