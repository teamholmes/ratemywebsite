using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;

namespace MyApp.Business.Services
{
    public interface IHttpContextFactory
    {
        HttpContext Context { get; }
    }

    public class HttpContextFactory : IHttpContextFactory
    {
        public HttpContext Context
        {
            get { return HttpContext.Current; }
            set { HttpContext.Current = value; }
        }

    }

   
}
