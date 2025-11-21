using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EULib
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public RegistrationResult RegisterUser(
            string email,
            string password,
            string confirmPassword,
            string fullName)
        {
            // Валидация email
            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                return RegistrationResult.Failure("Неверный формат электронной почты");

            // Валидация пароля
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return RegistrationResult.Failure("Пароль должен содержать не менее 8 символов");

            // Проверка совпадения паролей
            if (password != confirmPassword)
                return RegistrationResult.Failure("Пароли не совпадают");

            // Валидация ФИО
            if (string.IsNullOrWhiteSpace(fullName) || fullName.Trim().Length < 2)
                return RegistrationResult.Failure("ФИО должно содержать не менее 2 символов");

            if (!ContainsOnlyLettersAndSpaces(fullName))
                return RegistrationResult.Failure("ФИО может содержать только буквы и пробелы");

            // Синхронная проверка email
            if (_userRepository.IsEmailExists(email))
                return RegistrationResult.Failure("Адрес электронной почты уже зарегистрирован");

            // Создание пользователя
            var user = new User
            {
                Email = email.Trim().ToLower(),
                Password = HashPassword(password),
                FullName = fullName.Trim(),
                Role = Role.Client,
                RegistrationDate = DateTime.UtcNow,
                IsActive = true
            };

       
            var created = _userRepository.CreateUser(user);
            if (!created)
                return RegistrationResult.Failure("Ошибка при создании пользователя");

            return RegistrationResult.Success(user);
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
              
                var trimmedEmail = email.Trim();
                var addr = new System.Net.Mail.MailAddress(trimmedEmail);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        private bool ContainsOnlyLettersAndSpaces(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            return text.All(c => char.IsLetter(c) || c == ' ' || c == '-' || c == '.');
        }

        private string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    public class RegistrationResult
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }
        public User User { get; }

        private RegistrationResult(bool isSuccess, string errorMessage, User user)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            User = user;
        }

        public static RegistrationResult Success(User user) => new RegistrationResult(true, string.Empty, user);
        public static RegistrationResult Failure(string errorMessage) => new RegistrationResult(false, errorMessage, null);
    }
}