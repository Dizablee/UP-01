using EuroLink.Models;

namespace EuroLink.Services
{
    public interface IUserRepository
    {
        User GetUserByEmail(string email);
        bool CreateUser(User user);
        bool IsEmailExists(string email);
    }
}