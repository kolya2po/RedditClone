using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Business.MapModels;
using Business.MapModels.Identity;
using FluentAssertions;
using Forum.WebApi.Models.Identity;
using Forum.WebApi.Models.Topic;
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

        [Test]
        public async Task Add_AddsNewTopicToDb()
        {
            // Arrange
            var userId = await AddAuthorizationHeaderToHttpClientAndGetUserIdAsync();

            var createTopicDto = new CreateTopicDto
            {
                AuthorId = userId,
                CommunityId = 1,
                Text = "new topic text",
                Title = "new topic"
            };

            var content = new StringContent(JsonConvert.SerializeObject(createTopicDto), Encoding.UTF8, "application/json");

            // Act
            var httpResponse = await _httpClient.PostAsync(RequestUri, content);

            // Assert
            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var createdTopic = JsonConvert.DeserializeObject<TopicModel>(stringResponse);

            createdTopic.AuthorId.Should().Be(createTopicDto.AuthorId);
            createdTopic.CommunityId.Should().Be(createTopicDto.CommunityId);
            createdTopic.Text.Should().BeEquivalentTo(createTopicDto.Text);
            createdTopic.Title.Should().BeEquivalentTo(createTopicDto.Title);
        }

        [Test]
        public async Task Update_UpdatesTopic()
        {
            // Arrange
            await AddAuthorizationHeaderToHttpClientAndGetUserIdAsync();

            var updateTopicDto = new UpdateTopicDto
            {
                AuthorId = 1,
                CommunityId = 1,
                Id = 1,
                Title = "new Topic 1",
                PostingDate = new DateTime(2022, 1, 22).ToLongTimeString(),
                Rating = 0
            };

            var content = new StringContent(JsonConvert.SerializeObject(updateTopicDto), Encoding.UTF8, "application/json");

            // Act
            var httpResponse = await _httpClient.PutAsync(RequestUri, content);

            // Assert
            httpResponse.EnsureSuccessStatusCode();

            var response = await _httpClient.GetAsync(RequestUri + $"{updateTopicDto.Id}");
            var stringResponse = await response.Content.ReadAsStringAsync();
            var topic = JsonConvert.DeserializeObject<TopicModel>(stringResponse);

            topic.Title.Should().BeEquivalentTo(updateTopicDto.Title);
        }

        [Test]
        public async Task Delete_DeletesTopic()
        {
            // Arrange
            await AddAuthorizationHeaderToHttpClientAndGetUserIdAsync();
            var topicId = 1;
            var initialCount = JsonConvert.DeserializeObject<IEnumerable<TopicModel>>(await (await _httpClient.GetAsync(RequestUri)).Content.ReadAsStringAsync()).Count();

            // Act
            var httpResponse = await _httpClient.DeleteAsync(RequestUri + $"{topicId}");

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var newCount = JsonConvert.DeserializeObject<IEnumerable<TopicModel>>(await (await _httpClient.GetAsync(RequestUri)).Content.ReadAsStringAsync()).Count();

            initialCount.Should().NotBe(newCount);
        }

        [Test]
        public async Task IncreaseRating_IncreasesTopicsRating()
        {
            // Arrange
            await AddAuthorizationHeaderToHttpClientAndGetUserIdAsync();
            var oldTopic = JsonConvert.DeserializeObject<TopicModel>(await (await _httpClient.GetAsync(RequestUri + "1")).Content.ReadAsStringAsync());

            // Act
            var httpResponse = await _httpClient.PutAsync(RequestUri + "1/increase-rating", null);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var newTopic = JsonConvert.DeserializeObject<TopicModel>(await (await _httpClient.GetAsync(RequestUri + "1")).Content.ReadAsStringAsync());

            newTopic.Rating.Should().BeGreaterThan(oldTopic.Rating);
        }

        [Test]
        public async Task DecreaseRating_DecreasesTopicsRating()
        {
            // Arrange
            await AddAuthorizationHeaderToHttpClientAndGetUserIdAsync();
            var oldTopic = JsonConvert.DeserializeObject<TopicModel>(await (await _httpClient.GetAsync(RequestUri + "1")).Content.ReadAsStringAsync());

            // Act
            var httpResponse = await _httpClient.PutAsync(RequestUri + "1/decrease-rating", null);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var newTopic = JsonConvert.DeserializeObject<TopicModel>(await (await _httpClient.GetAsync(RequestUri + "1")).Content.ReadAsStringAsync());

            newTopic.Rating.Should().BeLessThan(oldTopic.Rating);
        }

        private async Task<int> AddAuthorizationHeaderToHttpClientAndGetUserIdAsync()
        {
            var registrationDto = new RegistrationDto
            {
                Email = "user1@email.com",
                UserName = "user1",
                Password = "password"
            };

            var content = new StringContent(JsonConvert.SerializeObject(registrationDto), Encoding.UTF8, "application/json");
            var httpResponse = await _httpClient.PostAsync("api/users/registration", content);
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var userDto = JsonConvert.DeserializeObject<UserDto>(stringResponse);

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", userDto.Token);

            return userDto.Id;
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
