using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication.Controllers
{

    public class WebsiteReview
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public int DesignRating { get; set; }
        public int FunctionalityRating { get; set; }
        public int ContentRating { get; set; }
        public string Comment { get; set; }

        public WebsiteReview()
        {

        }

    }


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
                    Comment = "Comment 1",
                    ContentRating = 2,
                    DesignRating = 3,
                    FunctionalityRating = 4,
                    Name = "My special website",
                    URL = "www.google.com"

                });


            reviewWebsites.Add(
               new WebsiteReview()
               {
                   Id = 2,
                   Comment = "Comment 2",
                   ContentRating = 4,
                   DesignRating = 4,
                   FunctionalityRating = 1,
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
