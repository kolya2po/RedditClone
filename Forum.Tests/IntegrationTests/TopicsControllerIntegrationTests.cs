using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Business.MapModels;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Forum.Tests.IntegrationTests
{
    [TestFixture]
    public class TopicsControllerIntegrationTests
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _httpClient;
        private const string RequestUri = "api/topics/";

        [SetUp]
        public void Initialize()
        {
            _factory = new CustomWebApplicationFactory();
            _httpClient = _factory.CreateClient();
        }

        [Test]
        public async Task GetAll_ReturnsAllFromDb()
        {
            // Arrange
            var expected = GetTestTopicModels;

            // Act
            var httpResponse = await _httpClient.GetAsync(RequestUri);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<TopicModel>>(stringResponse);

            actual.Should().BeEquivalentTo(expected, opt =>
            {
                return opt.Excluding(c => c.CommentModels)
                    .Excluding(c => c.PostingDate)
                    .Excluding(c => c.CommunityName)
                    .Excluding(c => c.AuthorName);
            });
        }

        [Test]
        public async Task GetById_ReturnsTopicById()
        {
            // Arrange
            var expected = GetTestTopicModel;
            var topicId = 1;

            // Act
            var httpResponse = await _httpClient.GetAsync(RequestUri + topicId);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<TopicModel>(stringResponse);

            actual.Should().BeEquivalentTo(expected, opt =>
            {
                return opt.Excluding(c => c.CommentModels)
                    .Excluding(c => c.PostingDate);
            });
        }

        private static TopicModel GetTestTopicModel => new TopicModel
        {

            Id = 1,
            AuthorId = 1,
            CommunityId = 1,
            Title = "Topic 1",
            Text = "Text topic 1",
            PostingDate = new DateTime(2022, 1, 22).ToLongDateString(),
            AuthorName = "Name 1",
            CommentModels = new[]
           {
                new CommentModel { Id = 1, AuthorId = 1, AuthorName = "Author1" },
                new CommentModel { Id = 2, AuthorId = 2, AuthorName = "Author2" },
            },
            CommentsCount = 4,
            CommunityName = "Community 1"
        };

        private static IEnumerable<TopicModel> GetTestTopicModels =>
            new[]
            {
                new TopicModel
                {
                    Id = 1,
                    AuthorId = 1,
                    CommunityId = 1,
                    Title = "Topic 1",
                    Text = "Text topic 1",
                    PostingDate = new DateTime(2022, 1, 22).ToLongDateString(),
                    CommentsCount = 4
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
