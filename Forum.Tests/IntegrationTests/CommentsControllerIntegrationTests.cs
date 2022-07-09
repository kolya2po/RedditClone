using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Business.MapModels;
using Business.MapModels.Identity;
using FluentAssertions;
using Forum.WebApi.Models.Comment;
using Forum.WebApi.Models.Identity;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Forum.Tests.IntegrationTests
{
    [TestFixture]
    public class CommentsControllerIntegrationTests
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _httpClient;
        private const string RequestUri = "api/comments/";

        [SetUp]
        public void Initialize()
        {
            _factory = new CustomWebApplicationFactory();
            _httpClient = _factory.CreateClient();
        }

        [Test]
        public async Task Add_AddsNewCommentToTheTopic()
        {
            // Arrange
            var userId = await AddAuthorizationHeaderToHttpClientAndGetUserIdAsync();
            var createCommentDto = new CreateCommentDto
            {
                AuthorId = userId,
                Text = "Cool",
                TopicId = 1
            };

            var content = new StringContent(JsonConvert.SerializeObject(createCommentDto),
                Encoding.UTF8, "application/json");
            var initialCount = JsonConvert.DeserializeObject<TopicModel>(await (await _httpClient.GetAsync($"api/Topics/{1}")).Content.ReadAsStringAsync()).CommentsCount;

            // Act
            var httpResponse = await _httpClient.PostAsync(RequestUri, content);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var newCount = JsonConvert.DeserializeObject<TopicModel>(await (await _httpClient.GetAsync($"api/Topics/{1}")).Content.ReadAsStringAsync()).CommentsCount;
            newCount.Should().BeGreaterThan(initialCount);
        }

        [Test]
        public async Task Update_UpdatesComment()
        {
            // Arrange
            await AddAuthorizationHeaderToHttpClientAndGetUserIdAsync();
            var updateCommentDto = new UpdateCommentDto()
            {
                Id = 1,
                Text = "Comment1 New Text",
                AuthorId = 1,
                TopicId = 1
            };

            var content = new StringContent(JsonConvert.SerializeObject(updateCommentDto),
                Encoding.UTF8, "application/json");

            // Act
            var httpResponse = await _httpClient.PutAsync(RequestUri, content);

            // Assert
            httpResponse.EnsureSuccessStatusCode();

            var comment = JsonConvert.DeserializeObject<TopicModel>(await (await _httpClient.GetAsync($"api/Topics/{1}")).Content.ReadAsStringAsync()).CommentModels.First(c => c.Id == updateCommentDto.Id);

            comment.Text.Should().BeEquivalentTo(updateCommentDto.Text);
        }

        [Test]
        public async Task Delete_DeletesComment()
        {
            // Arrange
            await AddAuthorizationHeaderToHttpClientAndGetUserIdAsync();
            var commentId = 1;
            var topicId = 1;

            var initialCount = JsonConvert.DeserializeObject<TopicModel>(await (await _httpClient.GetAsync($"api/Topics/{topicId}")).Content.ReadAsStringAsync()).CommentsCount;
            // Act
            var httpResponse = await _httpClient.DeleteAsync(RequestUri + $"{commentId}");

            // Assert
            httpResponse.EnsureSuccessStatusCode();

            var newCount = JsonConvert.DeserializeObject<TopicModel>(await (await _httpClient.GetAsync($"api/Topics/{topicId}")).Content.ReadAsStringAsync()).CommentsCount;

            newCount.Should().BeLessThan(initialCount);
        }

        [Test]
        public async Task ReplyToComment_CreatesReply()
        {
            // Arrange
            var userId = await AddAuthorizationHeaderToHttpClientAndGetUserIdAsync();
            var reply = new CreateCommentDto
            {
                AuthorId = userId,
                Text = "new reply",
                TopicId = 1
            };

            var content = new StringContent(JsonConvert.SerializeObject(reply), Encoding.UTF8, "application/json");

            var initialCount = JsonConvert.DeserializeObject<TopicModel>(await (await _httpClient.GetAsync($"api/Topics/{1}")).Content.ReadAsStringAsync()).CommentsCount;

            // Act
            var httpResponse = await _httpClient.PostAsync(RequestUri + "1/reply", content);

            // Assert
            httpResponse.EnsureSuccessStatusCode();

            var newCount = JsonConvert.DeserializeObject<TopicModel>(await (await _httpClient.GetAsync($"api/Topics/{1}")).Content.ReadAsStringAsync()).CommentsCount;
            newCount.Should().BeGreaterThan(initialCount);
        }

        [Test]
        public async Task IncreaseRating_IncreasesCommentsRating()
        {
            // Arrange
            await AddAuthorizationHeaderToHttpClientAndGetUserIdAsync();
            var commentId = 1;

            var initialRating = JsonConvert.DeserializeObject<TopicModel>(await (await _httpClient.GetAsync("api/Topics/1")).Content.ReadAsStringAsync()).CommentModels.First(c => c.Id == commentId).Rating;

            // Act
            var httpResponse = await _httpClient.PutAsync(RequestUri + $"{commentId}/increase-rating", null);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var newRating = JsonConvert.DeserializeObject<TopicModel>(await (await _httpClient.GetAsync("api/Topics/1")).Content.ReadAsStringAsync()).CommentModels.First(c => c.Id == commentId).Rating;

            newRating.Should().BeGreaterThan(initialRating);
        }


        [Test]
        public async Task DecreaseRating_DecreasesCommentsRating()
        {
            // Arrange
            await AddAuthorizationHeaderToHttpClientAndGetUserIdAsync();
            var commentId = 1;

            var initialRating = JsonConvert.DeserializeObject<TopicModel>(await (await _httpClient.GetAsync("api/Topics/1")).Content.ReadAsStringAsync()).CommentModels.First(c => c.Id == commentId).Rating;

            // Act
            var httpResponse = await _httpClient.PutAsync(RequestUri + $"{commentId}/decrease-rating", null);

            // Assert
            httpResponse.EnsureSuccessStatusCode();

            var newRating = JsonConvert.DeserializeObject<TopicModel>(await (await _httpClient.GetAsync("api/Topics/1")).Content.ReadAsStringAsync()).CommentModels.First(c => c.Id == commentId).Rating;

            newRating.Should().BeLessThan(initialRating);
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
    }
}
