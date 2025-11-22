using EuroLink.Models;
using System.Collections.Generic;

namespace EuroLink.Services
{
    public interface IPhoneRepository
    {
        Phone? GetPhoneByBrandAndModel(string brand, string model);
        bool CreatePhone(Phone phone);
        bool PhoneExists(string brand, string model);
        List<Phone> GetAllPhones();

        Phone? GetPhoneById(int phoneId);
        string? DeletePhones(List<int> phoneIds);
    }
}