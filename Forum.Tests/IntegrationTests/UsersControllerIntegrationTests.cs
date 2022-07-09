using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Business.MapModels;
using Business.MapModels.Identity;
using FluentAssertions;
using Forum.WebApi.Models.Identity;
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


        [Test]
        public async Task Registration_RegistersNewUser()
        {
            // Arrange
            var registrationDto = new RegistrationDto
            {
                Email = "user1@email.com",
                UserName = "user1",
                Password = "password"
            };

            // Act
            var httpResponse = await RegisterUser(registrationDto);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var userDto = JsonConvert.DeserializeObject<UserDto>(stringResponse);
            userDto.Id.Should().NotBe(0);
            userDto.Token.Should().NotBeNull();
        }

        [Test]
        public async Task Login_LoginsUser()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "user1@email.com",
                Password = "password"
            };
            var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");

            var registrationDto = new RegistrationDto
            {
                Email = "user1@email.com",
                UserName = "user1",
                Password = "password"
            };
            await RegisterUser(registrationDto);

            // Act
            var httpResponse = await _httpClient.PostAsync(RequestUri + "login", content);

            // Assert
            httpResponse.EnsureSuccessStatusCode();
            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var userDto = JsonConvert.DeserializeObject<UserDto>(stringResponse);
            userDto.Id.Should().NotBe(0);
            userDto.Token.Should().NotBeNull();
        }

        [Test]
        public async Task Update_UpdatesUser()
        {
            // Arrange
            var registrationDto = new RegistrationDto
            {
                Email = "user1@email.com",
                UserName = "user1",
                Password = "password"
            };
            var response = await (await RegisterUser(registrationDto)).Content.ReadAsStringAsync();
            var userDto = JsonConvert.DeserializeObject<UserDto>(response);

            var updateUserDto = new UpdateUserDto
            {
                Id = userDto.Id,
                UserName = "newUser2"
            };

            var content = new StringContent(JsonConvert.SerializeObject(updateUserDto), Encoding.UTF8, "application/json");

            // Act
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", userDto.Token);
            var httpResponse = await _httpClient.PutAsync(RequestUri, content);

            // Assert
            httpResponse.EnsureSuccessStatusCode();

            var stringResponse = await (await _httpClient.GetAsync(RequestUri + $"{userDto.Id}")).Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<UserModel>(stringResponse);
            user.UserName.Should().BeEquivalentTo(updateUserDto.UserName);
        }

        private async Task<HttpResponseMessage>  RegisterUser(RegistrationDto registrationDto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(registrationDto), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(RequestUri + "registration", content);
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
