using Moq;
using UserService.Repositories.Contract.Entities;
using UserService.Repositories.Contract.Repositories;
using UserService.Services.Contract.Enums;
using UserService.Services.Contract.Models.Requests;
using AppUserService = UserService.Services.Implementation.UserService;

namespace UserService.Services.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private AppUserService _userService;
        private Mock<IUsersRepository> _mockRepo;

        [SetUp]
        public void SetUp()
        {
            _mockRepo = new Mock<IUsersRepository>();
            _userService = new AppUserService(_mockRepo.Object);
        }

        [Test]
        public void CreateUser_ShouldReturnFailed_WhenEmailIsInvalid()
        {
            // Arrange
            var request = new CreateUserRequest { Email = "invalidEmail", Name = "John Doe", Password = "12345", Role = UserRoleCode.User };

            // Act
            var result = _userService.CreateUser(request);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(ServerResultCode.InvalidEmail, result.Code);
        }

        [Test]
        public void CreateUser_ShouldReturnFailed_WhenUserAlreadyExists()
        {
            // Arrange
            var request = new CreateUserRequest { Email = "john@example.com", Name = "John Doe", Password = "12345", Role = UserRoleCode.User };
            _mockRepo.Setup(repo => repo.DoesExistByEmail(request.Email)).Returns(true);

            // Act
            var result = _userService.CreateUser(request);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(ServerResultCode.UserWithSuchEmailExists, result.Code);
        }

        [Test]
        public void CreateUser_ShouldReturnSuccess_WhenUserIsValid()
        {
            // Arrange
            var request = new CreateUserRequest { Email = "john@example.com", Name = "John Doe", Password = "12345", Role = UserRoleCode.User };
            _mockRepo.Setup(repo => repo.DoesExistByEmail(request.Email)).Returns(false);

            // Act
            var result = _userService.CreateUser(request);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("User has been created", result.Message);
            _mockRepo.Verify(repo => repo.Add(It.IsAny<UserEntity>()), Times.Once);
        }

        [Test]
        public async Task GetUsersAsync_ShouldReturnUsers()
        {
            // Arrange
            var users = new List<UserEntity> { new UserEntity("John Doe", "john@example.com", "hashed_password", (int)UserRoleCode.User) };
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetUsersAsync();

            // Assert
            Assert.IsNotEmpty(result.Users);
            Assert.AreEqual("John Doe", result.Users.First().Name);
        }

        [Test]
        public async Task UpdateUserAsync_ShouldReturnFailed_WhenUserDoesNotExist()
        {
            // Arrange
            var request = new UpdateUserRequest { Id = 1, Email = "john@example.com", Name = "John Doe" };
            _mockRepo.Setup(repo => repo.GetUserByIdAsync(request.Id)).ReturnsAsync((UserEntity)null);

            // Act
            var result = await _userService.UpdateUserAsync(request);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(ServerResultCode.UserWithSuchIdDoesNotExist, result.Code);
        }

        [Test]
        public async Task UpdateUserAsync_ShouldReturnSuccess_WhenUserIsUpdated()
        {
            // Arrange
            var request = new UpdateUserRequest { Id = 1, Email = "john@example.com", Name = "John Updated" };
            var existingUser = new UserEntity("John Doe", "john@example.com", "hashed_password", (int)UserRoleCode.User);
            _mockRepo.Setup(repo => repo.GetUserByIdAsync(request.Id)).ReturnsAsync(existingUser);

            // Act
            var result = await _userService.UpdateUserAsync(request);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("User John Updated has been updated", result.Message);
            _mockRepo.Verify(repo => repo.UpdateUserAsync(existingUser), Times.Once);
        }
    }
}