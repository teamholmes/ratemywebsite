using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OP.General;

namespace MyApp.Business.DomainObjects.Models
{
    public class PassphraseHistory 
    {
        public string Id { get; set; }

        public string AspNetUsersId { get; set; }

        public DateTime DateOfChange { get; set; }

        public string PassphraseHash { get; set; }


        public PassphraseHistory()
        {

        }

    }


     
}
