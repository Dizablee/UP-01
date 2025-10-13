using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EULib
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> CreateUserAsync(User user);
        Task<bool> IsEmailExistsAsync(string email);
    }
}
