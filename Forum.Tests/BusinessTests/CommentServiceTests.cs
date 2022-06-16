using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.MapModels;
using Business.Services;
using Data.Interfaces;
using Data.Models;
using Moq;
using NUnit.Framework;

namespace Forum.Tests.BusinessTests
{
    [TestFixture]
    public class CommentServiceTests
    {
        [Test]
        public async Task AddAsync_AddCommentToDatabase()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.CommentRepository.AddAsync(It.IsAny<Comment>()));
            var commentService = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());

            var commentModel = GetTestCommentModels().First();

            // Act
            await commentService.AddAsync(commentModel);

            // Assert
            mockUnitOfWork.Verify(u => u.CommentRepository.AddAsync(It.Is<Comment>(c => c.AuthorId == commentModel.AuthorId && c.Id == commentModel.Id && c.Text == commentModel.Text)), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_UpdatesComment()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.CommentRepository.Update(It.IsAny<Comment>()));
            var commentService = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());
            var commentModel = GetTestCommentModels().First();

            // Act
            await commentService.UpdateAsync(commentModel);

            // Assert
            mockUnitOfWork.Verify(u => u.CommentRepository.Update(It.Is<Comment>(c => c.AuthorId == commentModel.AuthorId && c.Id == commentModel.Id && c.Text == commentModel.Text)), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_RemovesCommentFromDatabase()
        {
            // Arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(u => u.CommentRepository.DeleteByIdAsync(1));
            var commentService = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());

            // Act
            await commentService.DeleteAsync(1);

            // Assert
            mockUnitOfWork.Verify(u => u.CommentRepository.DeleteByIdAsync(1), Times.Once);
            mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }

        //[TestCase(1)]
        //public async Task GetAllFromTopicAsync_ReturnsCommentsThatBelongToTopic(int topicId)
        //{
        //    // Arrange
        //    var mockUnitOfWork = new Mock<IUnitOfWork>();
        //    var topic = new Topic
        //    {
        //        Id = topicId,
        //        Comments = UnitTestHelper.GetTestComments()
        //    };
        //    mockUnitOfWork.Setup(u => u.TopicRepository
        //            .GetByIdWithDetailsAsync(topicId))
        //            .ReturnsAsync(topic);
        //    var commentService = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile(), null);
        //    var expected = GetTestCommentModels();

        //    // Act
        //    var actual = await commentService.GetAllFromTopicAsync(topicId);

        //    // Assert
        //    actual.Should().BeEquivalentTo(expected);
        //}

        //[TestCase(1)]
        //public async Task GetAllFromUserAsync_ReturnsAllCommentsThatBelongToUser(int userId)
        //{
        //    // Arrange
        //    var user = new User
        //    {
        //        Id = userId,
        //        Comments = UnitTestHelper.GetTestComments().Where(c => c.AuthorId == userId)
        //    };
        //    var mockUserManager = UnitTestHelper.GetMockUserManager();
        //    mockUserManager.Setup(u => u.FindByIdAsync(userId.ToString()))
        //        .ReturnsAsync(user);
        //    var commentService = new CommentService(null, UnitTestHelper.CreateMapperFromProfile(), mockUserManager.Object);
        //    var expected = GetTestCommentModels().Where(c => c.AuthorId == userId);
        //    // Act
        //    var actual = await commentService.GetAllFromUserAsync(userId);

        //    // Assert
        //    actual.Should().BeEquivalentTo(expected);
        //}


        private static IEnumerable<CommentModel> GetTestCommentModels() =>
            new[]
            {
                new CommentModel
                {
                    Id = 1,
                    Rating = 1,
                    Text = "Comment1 Text",
                    AuthorId = 1,
                    TopicId = 1
                },
                new CommentModel
                {
                    Id = 2,
                    Rating = 22,
                    Text = "Comment2 Text",
                    AuthorId = 2,
                    TopicId = 1
                },
                new CommentModel
                {
                    Id = 3,
                    Rating = -5,
                    Text = "Comment3 Text",
                    AuthorId = 3,
                    TopicId = 1
                },
                new CommentModel
                {
                    Id = 4,
                    Rating = 4,
                    Text = "Comment4 Text",
                    AuthorId = 4,
                    TopicId = 1
                }
            };
    }
}
