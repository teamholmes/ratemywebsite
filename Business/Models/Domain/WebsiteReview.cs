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
        public DateTime DateAdded { get; set; }

        public virtual List<WebsiteReviewDetail> Reviews { get; set; }

        public double AverageDesignRating 
        {
            get
            {
                return Reviews.Any() ? Reviews.Average(x => x.DesignRating) : 0;
                
            }
            set { }
        }

        public double AverageContentRating 
        {
            get
            {
                return Reviews.Any() ? Reviews.Average(x => x.ContentRating) : 0;

            }
            set { }
        }

        public double AverageFunctionalityRating 
        {
            get
            {
                return Reviews.Any() ? Reviews.Average(x => x.FunctionalityRating) : 0;

            }
            set { }
        }

        public WebsiteReview()
        {
            Reviews = new List<WebsiteReviewDetail>();
        }

    }



}
