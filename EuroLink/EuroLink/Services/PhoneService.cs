using EuroLink.Models;
using EuroLink.Services;

namespace EuroLink.Services
{
    public class PhoneService
    {
        private readonly IPhoneRepository _phoneRepository;

        public PhoneService(IPhoneRepository phoneRepository)
        {
            _phoneRepository = phoneRepository;
        }

        public async Task<OperationResult> AddPhoneToCatalogAsync(
            string brand,
            string model,
            decimal price,
            int stockQuantity,
            string? color = null,
            string? memory = null,
            string? description = null,
            string? imageUrl = null)
        {
            // Валидация обязательных полей
            if (string.IsNullOrWhiteSpace(brand))
                return OperationResult.Failure("Бренд является обязательным полем");

            if (string.IsNullOrWhiteSpace(model))
                return OperationResult.Failure("Модель является обязательным полем");

            // Валидация цены
            if (price < 0)
                return OperationResult.Failure("Цена не может быть отрицательной");
            if (price == 0)
                return OperationResult.Failure("Цена должна быть больше нуля");

            // Валидация количества
            if (stockQuantity < 0)
                return OperationResult.Failure("Количество не может быть отрицательным");

            // Проверка на дубликат
            if (await _phoneRepository.PhoneExistsAsync(brand.Trim(), model.Trim()))
                return OperationResult.Failure("Телефон с такой комбинацией бренда и модели уже существует");

            // Создание телефона
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

            // Сохранение в БД
            var created = await _phoneRepository.CreatePhoneAsync(phone);
            if (!created)
                return OperationResult.Failure("Ошибка при сохранении телефона в базу данных");

            return OperationResult.Success(phone);
        }
    }
}