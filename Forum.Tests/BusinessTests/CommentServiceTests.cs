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

            var commentModel = GetTestCommentModels.First();

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
            var commentModel = GetTestCommentModels.First();

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

        [TestCase(1)]
        public async Task ReplyToCommentAsync_CreatesNewCommentAsReply(int id)
        {
            // Arrange
            var expected = new Comment
            {
                Id = id,
                Rating = 1,
                Text = "Comment1 Text",
                AuthorId = 1,
                Author = new User { Id = 1, UserName = "Author 1" },
                TopicId = 1
            };
            var reply = GetTestCommentModels.First();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(uof =>
                uof.CommentRepository.GetByIdWithDetailsAsync(id)).ReturnsAsync(expected);
            var commentService = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());

            // Act
            await commentService.ReplyToCommentAsync(id, reply);

            // Assert
            mockUnitOfWork.Verify(uof => uof.CommentRepository
                .AddAsync(It.Is<Comment>(c => c.Id == reply.Id)), Times.Once);
            mockUnitOfWork.Verify(uof => uof.SaveAsync(), Times.Once);
        }

        [TestCase(1)]
        public async Task IncreaseRatingAsync_IncreasesCommentsRating(int id)
        {
            // Arrange
            var expected = new Comment
            {
                Id = id,
                Rating = 1,
                Text = "Comment1 Text",
                AuthorId = 1,
                TopicId = 1
            };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(uof => uof.CommentRepository.GetByIdAsync(id)).ReturnsAsync(expected);
            var commentService = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());

            // Act
            await commentService.IncreaseRatingAsync(id);

            // Assert
            mockUnitOfWork.Verify(uof => uof.CommentRepository
                .Update(It.Is<Comment>(c => c.Id == expected.Id)), Times.Once);
            mockUnitOfWork.Verify(uof => uof.SaveAsync(), Times.Once);
        }

        [TestCase(1)]
        public async Task DecreaseRatingAsync_DecreasesCommentsRating(int id)
        {
            // Arrange
            var expected = new Comment
            {
                Id = id,
                Rating = 1,
                Text = "Comment1 Text",
                AuthorId = 1,
                TopicId = 1
            };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(uof => uof.CommentRepository.GetByIdAsync(id)).ReturnsAsync(expected);
            var commentService = new CommentService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperFromProfile());

            // Act
            await commentService.DecreaseRatingAsync(id);

            // Assert
            mockUnitOfWork.Verify(uof => uof.CommentRepository
                .Update(It.Is<Comment>(c => c.Id == expected.Id)), Times.Once);
            mockUnitOfWork.Verify(uof => uof.SaveAsync(), Times.Once);
        }


        private static IEnumerable<CommentModel> GetTestCommentModels =>
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
