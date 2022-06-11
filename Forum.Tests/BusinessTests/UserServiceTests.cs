using System.Threading.Tasks;
using Business.MapModels.Identity;
using Business.Services;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;

namespace Forum.Tests.BusinessTests
{
    [TestFixture]
    public class UserServiceTests
    {
        [Test]
        public async Task RegistrationAsync_AddsUserToDatabase()
        {
            // Arrange
            var mockUserManager = UnitTestHelper.GetMockUserManager();
            var mockSignInManager = UnitTestHelper.GetMockSignInManager();
            mockUserManager.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            mockSignInManager.Setup(s => s.SignInAsync(It.IsAny<User>(), false, null));
            var registrationModel = new RegistrationModel
            {
                Email = "cool@email.com",
                UserName = "User 1",
                Password = "password123"
            };

            var userService = new UserService(null, mockUserManager.Object, mockSignInManager.Object, UnitTestHelper.CreateMapperFromProfile(), null);

            // Act
            await userService.RegistrationAsync(registrationModel);

            // Assert
            mockUserManager.Verify(um => um.CreateAsync(It.Is<User>(u => u.Email == registrationModel.Email && u.UserName == registrationModel.UserName)));
            mockSignInManager.Verify(s => s.SignInAsync(It.Is<User>(u => u.Email == registrationModel.Email && u.UserName == registrationModel.UserName), false, null));
        }
    }
}
