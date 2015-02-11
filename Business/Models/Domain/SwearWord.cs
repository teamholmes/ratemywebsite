using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Business.DomainObjects.Models
{
    public class SwearWord
    {
        public int Id { get; set; }
        public string Word { get; set; }


        public SwearWord()
        {

        }

    }


}
