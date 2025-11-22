using EuroLink.Models;

namespace EuroLink.Services
{
    public class PhoneService(IPhoneRepository phoneRepository)
    {
        private readonly IPhoneRepository _phoneRepository = phoneRepository;

        public OperationResult AddPhoneToCatalog(
            string brand,
            string model,
            decimal price,
            int stockQuantity,
            string? color = null,
            string? memory = null,
            string? description = null,
            string? imageUrl = null)
        {
            // Валидация и бизнес-логика
            if (string.IsNullOrWhiteSpace(brand))
                return OperationResult.Failure("Бренд является обязательным полем");

            if (string.IsNullOrWhiteSpace(model))
                return OperationResult.Failure("Модель является обязательным полем");

            if (price < 0)
                return OperationResult.Failure("Цена не может быть отрицательной");
            if (price == 0)
                return OperationResult.Failure("Цена должна быть больше нуля");

            if (stockQuantity < 0)
                return OperationResult.Failure("Количество не может быть отрицательным");

            if (_phoneRepository.PhoneExists(brand.Trim(), model.Trim()))
                return OperationResult.Failure("Телефон с такой комбинацией бренда и модели уже существует");

            var phone = new Phone
            {
                Brand = brand.Trim(),
                Model = model.Trim(),
                Price = price,
                StockQuantity = stockQuantity,
                Color = color?.Trim(),
                Memory = memory?.Trim(),
                Description = description?.Trim(),
                ImageUrl = imageUrl?.Trim()
            };

            var created = _phoneRepository.CreatePhone(phone);
            if (!created)
                return OperationResult.Failure("Ошибка при сохранении телефона в базу данных");

            return OperationResult.Success(phone);
        }
        public string? DeletePhonesFromCatalog(List<int> phoneIds)
        {
            // Валидация входных данных
            var validationError = ValidatePhoneIds(phoneIds);
            if (validationError != null)
                return validationError;

            // Выполняем удаление через репозиторий
            return _phoneRepository.DeletePhones(phoneIds);
        }

        private string? ValidatePhoneIds(List<int> phoneIds)
        {
            if (phoneIds == null || phoneIds.Count == 0)
                return "Не выбрано ни одного телефона для удаления";

            if (phoneIds.Any(id => id <= 0))
                return "Указаны невалидные идентификаторы телефонов";

            return null; // Валидация пройдена
        }
    }
}