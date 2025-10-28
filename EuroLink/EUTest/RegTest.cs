using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using EULib;
using Assert = Xunit.Assert;

namespace EUTest
{
    [TestClass]
    public class RegTest
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;

        public RegTest()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _userService = new UserService(_mockUserRepository.Object);
        }

        [Fact]
        public async Task RegisterUserAsync_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var email = "ivan.petrov@example.com";
            var password = "Qwerty123";
            var fullName = "Иван Петров";

            _mockUserRepository
                .Setup(repo => repo.IsEmailExistsAsync(email))
                .ReturnsAsync(false);

            _mockUserRepository
                .Setup(repo => repo.CreateUserAsync(It.IsAny<User>()))
                .ReturnsAsync(true);

            // Act
            var result = await _userService.RegisterUserAsync(email, password, password, fullName);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.User);
            Assert.Equal(email, result.User.Email);
            Assert.Equal(fullName, result.User.FullName);
            Assert.Equal(Role.Client, result.User.Role);
            Assert.True(result.User.IsActive);

            _mockUserRepository.Verify(repo => repo.IsEmailExistsAsync(email), Times.Once);
            _mockUserRepository.Verify(repo => repo.CreateUserAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task RegisterUserAsync_WithDuplicateEmail_ReturnsFailure()
        {
            // Arrange
            var email = "existing.user@example.com";
            var password = "Qwerty123";
            var fullName = "Иван Иванов";

            _mockUserRepository
                .Setup(repo => repo.IsEmailExistsAsync(email))
                .ReturnsAsync(true);

            // Act
            var result = await _userService.RegisterUserAsync(email, password, password, fullName);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Адрес электронной почты уже зарегистрирован", result.ErrorMessage);

            _mockUserRepository.Verify(repo => repo.IsEmailExistsAsync(email), Times.Once);
            _mockUserRepository.Verify(repo => repo.CreateUserAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task RegisterUserAsync_WithPasswordMismatch_ReturnsFailure()
        {
            // Arrange
            var email = "test.user@example.com";
            var password = "Qwerty123";
            var confirmPassword = "Qwerty456";
            var fullName = "Петр Сидоров";

            // Act
            var result = await _userService.RegisterUserAsync(email, password, confirmPassword, fullName);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Пароли не совпадают", result.ErrorMessage);

            _mockUserRepository.Verify(repo => repo.IsEmailExistsAsync(It.IsAny<string>()), Times.Never);
            _mockUserRepository.Verify(repo => repo.CreateUserAsync(It.IsAny<User>()), Times.Never);
        }

        [Theory]
        [InlineData("ivan@@example")]
        [InlineData("invalid-email")]
        [InlineData("")]
        [InlineData(null)]
        public async Task RegisterUserAsync_WithInvalidEmail_ReturnsFailure(string invalidEmail)
        {
            // Arrange
            var password = "Qwerty123";
            var fullName = "Иван Петров";

            // Act
            var result = await _userService.RegisterUserAsync(invalidEmail, password, password, fullName);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Неверный формат электронной почты", result.ErrorMessage);

            _mockUserRepository.Verify(repo => repo.IsEmailExistsAsync(It.IsAny<string>()), Times.Never);
            _mockUserRepository.Verify(repo => repo.CreateUserAsync(It.IsAny<User>()), Times.Never);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("short")]
        [InlineData("")]
        [InlineData(null)]
        public async Task RegisterUserAsync_WithWeakPassword_ReturnsFailure(string weakPassword)
        {
            // Arrange
            var email = "simple.pass@example.com";
            var fullName = "Иван Петров";

            // Act
            var result = await _userService.RegisterUserAsync(email, weakPassword, weakPassword, fullName);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Пароль должен содержать не менее 8 символов", result.ErrorMessage);

            _mockUserRepository.Verify(repo => repo.IsEmailExistsAsync(It.IsAny<string>()), Times.Never);
            _mockUserRepository.Verify(repo => repo.CreateUserAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task RegisterUserAsync_WithDigitsInFullName_ReturnsFailure()
        {
            // Arrange
            var email = "fio.test@example.com";
            var password = "Qwerty123";
            var fullName = "Иван123 Петров";

            // Act
            var result = await _userService.RegisterUserAsync(email, password, password, fullName);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("ФИО может содержать только буквы и пробелы", result.ErrorMessage);

            _mockUserRepository.Verify(repo => repo.IsEmailExistsAsync(It.IsAny<string>()), Times.Never);
            _mockUserRepository.Verify(repo => repo.CreateUserAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task RegisterUserAsync_WithShortFullName_ReturnsFailure()
        {
            // Arrange
            var email = "short.name@example.com";
            var password = "Qwerty123";
            var fullName = "Я";

            // Act
            var result = await _userService.RegisterUserAsync(email, password, password, fullName);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("ФИО должно содержать не менее 2 символов", result.ErrorMessage);

            _mockUserRepository.Verify(repo => repo.IsEmailExistsAsync(It.IsAny<string>()), Times.Never);
            _mockUserRepository.Verify(repo => repo.CreateUserAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task RegisterUserAsync_WithEmptyRequiredFields_ReturnsFailure()
        {
            // Arrange
            var email = "";
            var password = "";
            var confirmPassword = "";
            var fullName = "";

            // Act
            var result = await _userService.RegisterUserAsync(email, password, confirmPassword, fullName);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Неверный формат электронной почты", result.ErrorMessage);

            _mockUserRepository.Verify(repo => repo.IsEmailExistsAsync(It.IsAny<string>()), Times.Never);
            _mockUserRepository.Verify(repo => repo.CreateUserAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task RegisterUserAsync_WhenRepositoryFails_ReturnsFailure()
        {
            // Arrange
            var email = "test@example.com";
            var password = "Qwerty123";
            var fullName = "Иван Петров";

            _mockUserRepository
                .Setup(repo => repo.IsEmailExistsAsync(email))
                .ReturnsAsync(false);

            _mockUserRepository
                .Setup(repo => repo.CreateUserAsync(It.IsAny<User>()))
                .ReturnsAsync(false);

            // Act
            var result = await _userService.RegisterUserAsync(email, password, password, fullName);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Ошибка при создании пользователя", result.ErrorMessage);

            _mockUserRepository.Verify(repo => repo.IsEmailExistsAsync(email), Times.Once);
            _mockUserRepository.Verify(repo => repo.CreateUserAsync(It.IsAny<User>()), Times.Once);
        }

        [Theory]
        [InlineData("Мария-Анна Иванова")]
        [InlineData("Jean Paul Gaultier")]
        [InlineData("Анна Мария")]
        [InlineData("Иван Петров")]
        public async Task RegisterUserAsync_WithValidFullNames_ReturnsSuccess(string validFullName)
        {
            // Arrange
            var email = "test@example.com";
            var password = "Qwerty123";

            _mockUserRepository
                .Setup(repo => repo.IsEmailExistsAsync(email))
                .ReturnsAsync(false);

            _mockUserRepository
                .Setup(repo => repo.CreateUserAsync(It.IsAny<User>()))
                .ReturnsAsync(true);

            // Act
            var result = await _userService.RegisterUserAsync(email, password, password, validFullName);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(validFullName, result.User.FullName);
        }

        [Theory]
        [InlineData("Иван 123Петров")]
        [InlineData("Иван@ Петров")]
        [InlineData("Иван_Петров")]
        public async Task RegisterUserAsync_WithInvalidCharactersInFullName_ReturnsFailure(string invalidFullName)
        {
            // Arrange
            var email = "test@example.com";
            var password = "Qwerty123";

            // Act
            var result = await _userService.RegisterUserAsync(email, password, password, invalidFullName);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("ФИО может содержать только буквы и пробелы", result.ErrorMessage);

            _mockUserRepository.Verify(repo => repo.IsEmailExistsAsync(It.IsAny<string>()), Times.Never);
            _mockUserRepository.Verify(repo => repo.CreateUserAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task RegisterUserAsync_TrimsWhitespaceFromInputs_ReturnsSuccess()
        {
            // Arrange
            var email = "  ivan.petrov@example.com  ";
            var password = "Qwerty123";
            var fullName = "  Иван Петров  ";

            _mockUserRepository
                .Setup(repo => repo.IsEmailExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            _mockUserRepository
                .Setup(repo => repo.CreateUserAsync(It.IsAny<User>()))
                .ReturnsAsync(true);

            // Act
            var result = await _userService.RegisterUserAsync(email, password, password, fullName);
            if (!result.IsSuccess)
            {
                Console.WriteLine($"ОШИБКА РЕГИСТРАЦИИ: {result.ErrorMessage}");
            }
            Assert.True(result.IsSuccess, $"Регистрация не удалась: {result.ErrorMessage}");
            Assert.Equal("ivan.petrov@example.com", result.User.Email);
            Assert.Equal("Иван Петров", result.User.FullName);
        }
    }
}
