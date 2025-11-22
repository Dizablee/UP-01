using System.Net.Mail;
using EuroLink.Models;

namespace EuroLink.Services
{
    public class UserService(IUserRepository userRepository)
    {
        private readonly IUserRepository _userRepository = userRepository;

        public (bool success, string message, User? user) RegisterUser(
            string email,
            string password,
            string confirmPassword,
            string fullName)
        {
            // Валидация email
            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                return (false, "Неверный формат электронной почты", null);

            // Валидация пароля
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
                return (false, "Пароль должен содержать не менее 8 символов", null);

            // Проверка совпадения паролей
            if (password != confirmPassword)
                return (false, "Пароли не совпадают", null);

            // Валидация ФИО
            if (string.IsNullOrWhiteSpace(fullName) || fullName.Trim().Length < 2)
                return (false, "ФИО должно содержать не менее 2 символов", null);

            if (!ContainsOnlyLettersAndSpaces(fullName))
                return (false, "ФИО может содержать только буквы и пробелы", null);

            // Синхронная проверка email
            if (_userRepository.IsEmailExists(email))
                return (false, "Адрес электронной почты уже зарегистрирован", null);

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
                return (false, "Ошибка при создании пользователя", null);

            return (true, "Пользователь успешно зарегистрирован", user);
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var trimmedEmail = email.Trim();
                var addr = new MailAddress(trimmedEmail);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        private static bool ContainsOnlyLettersAndSpaces(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            return text.All(c => char.IsLetter(c) || c == ' ' || c == '-' || c == '.');
        }

        private static string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}