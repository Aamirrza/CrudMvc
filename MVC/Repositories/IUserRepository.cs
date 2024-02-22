using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC.Models;

namespace MVC.Repositories
{
    public interface IUserRepository
    {
        void UserRegister(tblUser user);

        bool Login(tblUser user);

        bool EmailExists(string email);

        bool ValidPassword(string password);
    }
}