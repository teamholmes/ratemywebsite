using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OP.General;

namespace MyApp.Business.DomainObjects.Models
{
    public class WebsiteReviewDetail
    {

        public int Id { get; set; }
        public int WebsiteReviewId { get; set; }
        public string DesignRating { get; set; }
        public string FunctionalityRating { get; set; }
        public string ContentRating { get; set; }
        public string Comment { get; set; }

        public WebsiteReviewDetail()
        {

        }

    }



}
