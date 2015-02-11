using System;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Linq;
using MyApp.Business.Resource;
using OP.General.Encryption;
using OP.General.Extensions;
using MyApp.Business.DomainObjects.Models;
using OP.General.Dal;
using OP.General.Model;
using System.Web;
using System.Collections.Generic;

namespace MyApp.Business.Services
{
    public class SessionService : ISessionService
    {

        private IConfiguration _Configuration;
        private ILog _Log;
        private IHttpContextFactory _Context;

        private T GetSession<T>(string key)
        {
            //return (T)HttpContext.Current.Session[key];
            return (T)_Context.Context.Session[key];

        }
        // see this page
        //http://forums.asp.net/t/1314952.aspx
        private void SetSession(string key, object value)
        {
            //HttpContext.Current.Session[key] = value;
            _Context.Context.Session[key] = value;
        }

        //public SessionService(HttpContext context, IConfiguration configuration, ILog log)
        public SessionService(IHttpContextFactory context, IConfiguration configuration, ILog log)
        {
            _Configuration = configuration;
            _Log = log;
            _Context = context;

        }

        public SessionModel SessionModel
        {
            get
            {
                if (GetSession<SessionModel>("SESSION_SESSIONMODEL") == null)
                {
                    SetSession("SESSION_SESSIONMODEL", new SessionModel());
                }
                return GetSession<SessionModel>("SESSION_SESSIONMODEL");
            }
            set { SetSession("SESSION_SESSIONMODEL", value); }
        }





    }
}
