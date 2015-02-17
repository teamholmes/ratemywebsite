using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OP.General;

namespace MyApp.Business.DomainObjects.Models
{
  
    public class WebsiteReviewDetail
    {

        public string Id { get; set; }
        public string WebsiteReviewId { get; set; }
        public int DesignRating { get; set; }
        public int FunctionalityRating { get; set; }
        public int ContentRating { get; set; }
        public string Comment { get; set; }


        public virtual WebsiteReview WebsiteReview { get; set; }

        public WebsiteReviewDetail()
        {

        }

    }



}
