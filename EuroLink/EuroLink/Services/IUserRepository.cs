using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EULib
{
    public interface IUserRepository
    {
        User GetUserByEmail(string email);
        bool CreateUser(User user);
        bool IsEmailExists(string email);
    }
}