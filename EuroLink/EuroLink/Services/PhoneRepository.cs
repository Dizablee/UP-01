using Microsoft.EntityFrameworkCore;
using EuroLink.Data;
using EuroLink.Models;

namespace EuroLink.Services
{
    public class PhoneRepository : IPhoneRepository
    {
        private readonly ApplicationDbContext _context;

        public PhoneRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Phone?> GetPhoneByBrandAndModelAsync(string brand, string model)
        {
            return await _context.Phones
                .FirstOrDefaultAsync(p => p.Brand == brand && p.Model == model);
        }

        public async Task<bool> PhoneExistsAsync(string brand, string model)
        {
            return await _context.Phones
                .AnyAsync(p => p.Brand == brand && p.Model == model);
        }

        public async Task<bool> CreatePhoneAsync(Phone phone)
        {
            try
            {
                _context.Phones.Add(phone);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<List<Phone>> GetAllPhonesAsync()
        {
            return await _context.Phones
                .OrderBy(p => p.Brand)
                .ThenBy(p => p.Model)
                .ToListAsync();
        }
    }
}