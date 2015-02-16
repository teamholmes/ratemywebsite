using MyApp.Business.DomainObjects.Models;
using MyApp.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication.Controllers
{


    public class PublicController : ApiController
    {

       private IAccountService _AccountService;
        private IAdminService _AdminService;
        private IConfiguration _Configuration;
        private IWebsiteReviewService _WebsiteReviewService;


        public PublicController(IAccountService accountService, IAdminService adminService, IConfiguration configuration, IWebsiteReviewService webreviewservice)
        {
            _AccountService = accountService;
            _AdminService = adminService;
            _Configuration = configuration;
            _WebsiteReviewService = webreviewservice;

        }


        // GET api/public
        public List<WebsiteReview> Get()
        {
            string g = "41";

  
            return _WebsiteReviewService.GetAll(128);
        }

        // GET api/public/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/public
        public void Post([FromBody]string value)
        {
        }

        // PUT api/public/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/public/5
        public void Delete(int id)
        {
        }
    }
}
