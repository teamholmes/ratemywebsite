using MyApp.Business.DomainObjects;
using OP.General.Extensions;
using System.Collections.Generic;
using System.Linq;
using OP.General.Dal;
using MyApp.Business.Services;
using MyApp.Business.DomainObjects.Models;

namespace MyApp.Business.Services
{
    public class UserService : IUserService
    {
        private IRepository _Repository;

        public UserService(IRepository repository)
        {
            _Repository = repository;
        }

       
        public string GetEmailAddressForUser(int userId)
        {

          
            return "hellO";
        }

        public long GetUserIdFromUsername(string username)
        {

            //_Log.Debug(string.Format("GetUserIdFromUsername {0}", username));

            //UserProfile dalUser = _Repository.GetAll<UserProfile>()
            //    .Where(x => x.IsEnabled && x.Username == username)
            //    .SingleOrDefault();

            //if (dalUser == null)
            //    return default(int);

            //return dalUser.Id;
            return 1;
        }

        public UserProfile GetUserById(int id)
        {

            return _Repository.GetById<UserProfile>(id);
        }

        public List<UserProfile> GetAllUsers()
        {

            return _Repository.GetAll<UserProfile>().ToList();
        }
    }
}
