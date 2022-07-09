using Business.MapModels;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Business.MapModels.Identity;
using Forum.WebApi.Models.Community;
using Forum.WebApi.Models.Identity;

namespace Forum.Tests.IntegrationTests
{
    [TestFixture]
    public class CommunitiesControllerIntegrationTests
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _httpClient;
        private const string RequestUri = "api/communities/";

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
            var expected = GetTestCommunityModels.ToList();

            // Act
            var httpResponse = await _httpClient.GetAsync(RequestUri);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<IEnumerable<CommunityModel>>(stringResponse).ToList();

            actual.Should().BeEquivalentTo(expected, opt =>
                 opt.Excluding(c => c.PostModels)
                    .Excluding(c => c.RuleModels));
        }

        [Test]
        public async Task GetById_ReturnsCommunityById()
        {
            // Arrange
            var expected = GetTestCommunityModels.First();
            var communityId = 1;

            // Act
            var httpResponse = await _httpClient.GetAsync(RequestUri + communityId);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<CommunityModel>(stringResponse);

            actual.Should().BeEquivalentTo(expected, opt =>
                 opt.Excluding(c => c.PostModels)
                    .Excluding(c => c.ModeratorModels));

            actual.PostModels.Should().BeEquivalentTo(expected.PostModels, opt =>
                opt.Excluding(p => p.CommentModels));

            actual.ModeratorModels.Should().BeEquivalentTo(expected.ModeratorModels, opt =>
                opt.Excluding(m => m.CommentModels)
                    .Excluding(m => m.PostModels));
        }

        [Test]
        public async Task Add_AddsNewCommunityToDb()
        {
            // Arrange
            var userId = await AddAuthorizationHeaderToHttpClientAndGetUserIdAsync();
            var createCommunityDto = new CreateCommunityDto
            {
                CreatorId = userId,
                Title = "New Community",
                About = "About"
            };

            var content = new StringContent(JsonConvert.SerializeObject(createCommunityDto), Encoding.UTF8,
                "application/json");

            // Act
            var httpResponse = await _httpClient.PostAsync(RequestUri, content);

            // Assert
            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var community = JsonConvert.DeserializeObject<CommunityModel>(stringResponse);

            community.CreatorId.Should().Be(createCommunityDto.CreatorId);
            community.Title.Should().BeEquivalentTo(createCommunityDto.Title);
            community.About.Should().BeEquivalentTo(createCommunityDto.About);
        }

        [Test]
        public async Task Join_AddsUserToCommunityMembers()
        {
            // Arrange
            var userId = await AddAuthorizationHeaderToHttpClientAndGetUserIdAsync();
            var communityId = 3;

            // Act
            var httpResponse =
                await _httpClient.PostAsync(RequestUri + $"join?userId={userId}&communityId={communityId}", null);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var user = JsonConvert.DeserializeObject<UserModel>(await (await _httpClient.GetAsync($"api/Users/{userId}")).Content.ReadAsStringAsync());

            user.CommunitiesIds.Contains(communityId).Should().BeTrue();
        }

        [Test]
        public async Task Leave_RemovesUserFromCommunityMembers()
        {
            // Arrange
            await AddAuthorizationHeaderToHttpClientAndGetUserIdAsync();
            var userId = 4;
            var communityId = 2;

            // Act
            var httpResponse =
                await _httpClient.DeleteAsync(RequestUri + $"leave?userId={userId}&communityId={communityId}");

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var user = JsonConvert.DeserializeObject<UserModel>(await (await _httpClient.GetAsync($"api/Users/{userId}")).Content.ReadAsStringAsync());

            user.CommunitiesIds.Contains(communityId).Should().BeFalse();
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

        private static IEnumerable<CommunityModel> GetTestCommunityModels =>
            new[]
            {
                new CommunityModel
                {
                    Id = 1,
                    Title = "Community 1",
                    About = "About community 1",
                    CreationDate = new DateTime(2022, 1, 16).ToShortDateString(),
                    CreatorId = 1,
                    MembersCount = 2,
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
                                Text =  "Text topic 1",
                                Title = "Topic 1"
                            },
                            new TopicModel
                            {
                                AuthorId = 2,
                                AuthorName = "Name 2",
                                CommentModels = Enumerable.Empty<CommentModel>(),
                                CommunityId = 1,
                                CommunityName = "Community 1",
                                Id = 2,
                                PostingDate = "04.06.2022 0:00",
                                Text = "Text topic 2",
                                Title = "Topic 2"
                            }
                        },
                    RuleModels = new []
                    {
                        new RuleModel
                        {
                            CommunityId = 1,
                            Id = 1,
                            RuleText = "Never break rule 1",
                            Title = "Rule 1"
                        },
                        new RuleModel
                        {
                            CommunityId = 1,
                            Id = 2,
                            RuleText = "Never break rule 2",
                            Title = "Rule 2"
                        },
                        new RuleModel
                        {
                            CommunityId = 1,
                            Id = 3,
                            RuleText = "Never break rule 3",
                            Title = "Rule 3"
                        },
                        new RuleModel
                        {
                            CommunityId = 1,
                            Id = 4,
                            RuleText = "Never break rule 4",
                            Title = "Rule 4"
                        }
                    },
                    ModeratorModels = new []
                    {
                        new UserModel
                        {
                            BirthDate = "01.01.0001",
                            CreatedCommunityId = 1,
                            CommunitiesIds = new [] {1},
                            PostModels = Enumerable.Empty<TopicModel>(),
                            CommentModels = Enumerable.Empty<CommentModel>(),
                            Id = 1,
                            Karma = 0,
                            ModeratedCommunityId = 1,
                            UserName = "Name 1"
                        }
                    }
                },
                new CommunityModel
                {
                    Id = 2,
                    Title = "Community 2",
                    About = "About community 2",
                    CreationDate = new DateTime(2022, 1, 18).ToShortDateString(),
                    CreatorId = 2,
                    MembersCount = 2,
                    PostModels = Enumerable.Empty<TopicModel>(),
                    RuleModels = Enumerable.Empty<RuleModel>(),
                    ModeratorModels = new []
                    {
                        new UserModel
                        {
                            BirthDate =  "01.01.0001",
                            PostModels = Enumerable.Empty<TopicModel>(),
                            CommentModels = Enumerable.Empty<CommentModel>(),
                            CommunitiesIds = new [] {2},
                            CreatedCommunityId = 3,
                            Id = 3,
                            Karma = 0,
                            ModeratedCommunityId = 2,
                            UserName = "Name 3"
                        }
                    }
                },
                new CommunityModel
                {
                    Id = 3,
                    Title = "Community 3",
                    About = "About community 3",
                    CreationDate = new DateTime(2022, 3, 16).ToShortDateString(),
                    CreatorId = 3,
                    PostModels = Enumerable.Empty<TopicModel>(),
                    RuleModels = Enumerable.Empty<RuleModel>(),
                    ModeratorModels = Enumerable.Empty<UserModel>()
                }
            };
    }
}
