using MyApp.Business.DomainObjects.Models;
using System;
namespace MyApp.Business.Services
{
    public interface ISessionService
    {

        SessionModel SessionModel { get; set; }
       
    }
}
