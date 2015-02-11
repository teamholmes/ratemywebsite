using MyApp.Business.DomainObjects.Models;
using System;

namespace MyApp.Business.Services
{
    public interface IAppConfigurationService
    {
        T GetConfigurationByKey<T>(string keyName) where T : IConvertible;
        string AddConfigurationKeyValue(AppConfiguration appConfig);
    }
}
