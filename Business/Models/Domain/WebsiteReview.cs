using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OP.General;

namespace MyApp.Business.DomainObjects.Models
{

    public class WebsiteReview
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }

        //public virtual List<WebsiteReviewDetail> Reviews { get; set; }

        public WebsiteReview()
        {
           // Reviews = new List<WebsiteReviewDetail>();
        }

    }



}
