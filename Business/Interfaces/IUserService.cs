using System;
using System.Collections.Generic;
using MyApp.Business.DomainObjects.Models;

namespace MyApp.Business.Services
{
    public interface IUserService
    {
        string GetEmailAddressForUser(int userId);
        long GetUserIdFromUsername(string username);
        UserProfile GetUserById(int id);
        List<UserProfile> GetAllUsers();
    }
}
