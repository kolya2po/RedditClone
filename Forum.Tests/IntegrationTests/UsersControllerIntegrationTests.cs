using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Business.MapModels;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Forum.Tests.IntegrationTests
{
    [TestFixture]
    public class UsersControllerIntegrationTests
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _httpClient;
        private const string RequestUri = "api/users/";

        [SetUp]
        public void Initialize()
        {
            _factory = new CustomWebApplicationFactory();
            _httpClient = _factory.CreateClient();
        }

        [Test]
        public async Task GetById_ReturnsUserById()
        {
            // Arrange
            var expected = GetTestUserModel;
            var userId = 1;

            // Act
            var httpResponse = await _httpClient.GetAsync(RequestUri + userId);

            // Assert
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<UserModel>(stringResponse);

            actual.Should().BeEquivalentTo(expected, opt =>
                opt.Excluding(c => c.PostModels)
                    .Excluding(c => c.Token));

            actual.PostModels.Should().BeEquivalentTo(expected.PostModels, opt =>
                opt.Excluding(c => c.CommentModels));
        }

        private static UserModel GetTestUserModel =>
            new UserModel
            {
                Id = 1,
                UserName = "Name 1",
                ModeratedCommunityId = 1,
                CreatedCommunityId = 1,
                PostModels = new []
                {
                    new TopicModel
                    {
                        AuthorId = 1,
                        AuthorName = "Name 1",
                        CommentModels = Enumerable.Empty<CommentModel>(),
                        CommentsCount = 4,
                        CommunityId = 1,
                        CommunityName = "Community 1",
                        Id = 1,
                        PostingDate = "22.01.2022 0:00",
                        Rating = 0,
                        Text = "Text topic 1",
                        Title = "Topic 1"
                    }
                },
                CommentModels = new []
                {
                    new CommentModel
                    {
                        AuthorId = 1,
                        AuthorName = "Name 1",
                        CommunityName = "Community 1",
                        Id = 1,
                        PostingDate = "01.01.0001 0:00",
                        Rating = 1,
                        Text = "Comment1 Text",
                        TopicId = 1,
                        TopicName = "Topic 1"
                    }
                },
                BirthDate = "01.01.0001",
                Karma = 1,
                CommunitiesIds = new []{1}
            };
    }
}
