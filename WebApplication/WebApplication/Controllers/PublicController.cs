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
        // GET api/public
        public List<WebsiteReview> Get()
        {
            List<WebsiteReview> reviewWebsites = new List<WebsiteReview>();
            reviewWebsites.Add(
                new WebsiteReview()
                {
                    Id = 1,
                    Name = "My special website",
                    URL = "www.google.com"

                });


            reviewWebsites.Add(
               new WebsiteReview()
               {
                   Id = 2,
                   Name = "My special website2",
                   URL = "www.google.com2"

               });

            return reviewWebsites;

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
