using MyApp.Business.DomainObjects.Models;
using MyApp.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OP.General.Extensions;
using Newtonsoft.Json;

namespace WebApplication.Controllers
{
    public class AddReview
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }


    public class ResponsePacket
    {
        public Boolean Success { get; set; }
        public string Message { get; set; }
        public object ResultData { get; set; }
    }

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
        public ResponsePacket Get()
        {
            string g = "41";

            List<WebsiteReview> retval = _WebsiteReviewService.GetAll(128);

            ResponsePacket resp = new ResponsePacket()
            {
                ResultData = retval,
                Message = String.Format("{0} records retrieved",retval.Count),
                Success = true
            };

            return resp;

           // return new string[] { "apples", "bananas", "pears" };
        }

        // GET api/public/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/public
        [HttpPost]
        public HttpResponseMessage Post(AddReview data)
        {

            WebsiteReview newreview = new WebsiteReview()
            {
                Name = data.Name.TrimforUI(255, false),
                URL = data.Url.TrimforUI(255, false)
            };

            string response = _WebsiteReviewService.Add(newreview);

            ResponsePacket resp = new ResponsePacket()
               {
                   ResultData = string.Empty,
                   Message = "Failed to add record",
                   Success = false
               };

            if (response.IsNotNullOrEmpty())
            {
                resp = new ResponsePacket()
                {
                    ResultData = string.Empty,
                    Message = "Record added",
                    Success = true
                };
            }
            return Request.CreateResponse(HttpStatusCode.OK, resp);
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
