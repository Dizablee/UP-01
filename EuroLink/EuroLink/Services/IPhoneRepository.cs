using EuroLink.Models;

namespace EuroLink.Services
{
    public interface IPhoneRepository
    {
        Task<Phone?> GetPhoneByBrandAndModelAsync(string brand, string model);
        Task<bool> CreatePhoneAsync(Phone phone);
        Task<bool> PhoneExistsAsync(string brand, string model);
        Task<List<Phone>> GetAllPhonesAsync();
    }
}