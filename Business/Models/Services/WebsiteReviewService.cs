using MyApp.Business.DomainObjects;
using OP.General.Extensions;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using OP.General.Dal;
using MyApp.Business.DomainObjects.Models;
using System.Collections.Generic;
using WebMatrix.WebData;
using System.Web.Security;
using System.Web;
using System.Web.Helpers;
using System.Collections;
using OP.General.Model;
using System.Text.RegularExpressions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MyApp.Business.Services
{
    public class WebsiteReviewService : IWebsiteReviewService
    {
        private IRepository _Repository;
        private ILog _Log;
        private IConfiguration _Configuration;
        private IEmailService _EmailService;


        public WebsiteReviewService(IRepository repository, ILog log, IConfiguration configuration, IEmailService emailservice)
        {
            _Repository = repository;
            _Log = log;
            _Configuration = configuration;
            _EmailService = emailservice;
        }


        public List<WebsiteReview> GetAll(int? maxrows)
        {
            if (maxrows.HasValue)
            {
                return _Repository.GetFiltered<WebsiteReview>((x => x.Id != null)).Take(maxrows.Value).ToList();

            }

            return _Repository.GetFiltered<WebsiteReview>((x => x.Id != null)).ToList();
        }


        public string Add(WebsiteReview review)
        {
            review.Id = Guid.NewGuid().ToString();
            return _Repository.Add<WebsiteReview>(review);
        }




    }
}
