using MyApp.Business.DomainObjects;
using OP.General.Extensions;
using System;
using System.Linq;
using OP.General.Dal;
using MyApp.Business.Services;
using MyApp.Business.DomainObjects.Models;
using OP.General.Model;

namespace MyApp.Business.Services
{
    public class AppConfigurationService : IAppConfigurationService
    {
        private IRepository _Repository;

        public AppConfigurationService(IRepository repository, ILog log)
        {
            _Repository = repository;
        }

        public T GetConfigurationByKey<T>(string keyName) where T : IConvertible
        {

            string uniquekey = "GetConfigurationByKey_" + keyName;

            if (Utilities.GetFromCache(uniquekey) != null)
            {
                return (T)Utilities.GetFromCache(uniquekey);
            }
            else
            {

                AppConfiguration conf = _Repository.Find<AppConfiguration>(x => x.Key.Equals(keyName, StringComparison.InvariantCultureIgnoreCase))
              .SingleOrDefault();

                if (conf == null)
                {
                    return default(T);
                }

                Utilities.SetCache(uniquekey, (T)Convert.ChangeType(conf.Value, typeof(T)), 5);
                return (T)Utilities.GetFromCache(uniquekey);
            }


        }


        private bool DoesAppConfigurationAlreadyExist(AppConfiguration appConfig)
        {
            return GetConfigurationByKey<string>(appConfig.Key) != null;
        }


        public string AddConfigurationKeyValue(AppConfiguration appConfig)
        {

            if (!DoesAppConfigurationAlreadyExist(appConfig))
            {
                return _Repository.Add(appConfig);
            }

            return null;

        }

    }
}
