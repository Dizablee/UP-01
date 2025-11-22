using Microsoft.EntityFrameworkCore;
using EuroLink.Data;
using EuroLink.Models;
using System.Collections.Generic;
using System.Linq;

namespace EuroLink.Services
{
    public class PhoneRepository(ApplicationDbContext context) : IPhoneRepository
    {
        private readonly ApplicationDbContext _context = context;

        public Phone? GetPhoneByBrandAndModel(string brand, string model)
        {
            return _context.Phones
                .FirstOrDefault(p => p.Brand == brand && p.Model == model);
        }

        public bool PhoneExists(string brand, string model)
        {
            return _context.Phones
                .Any(p => p.Brand == brand && p.Model == model);
        }

        public bool CreatePhone(Phone phone)
        {
            try
            {
                _context.Phones.Add(phone);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public List<Phone> GetAllPhones()
        {
            return [.. _context.Phones
                .OrderBy(p => p.Brand)
                .ThenBy(p => p.Model)];
        }
    }
}