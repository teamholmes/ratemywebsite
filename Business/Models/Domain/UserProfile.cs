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
    public class UserProfile
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public string ContactNumber { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean IsTemporaryPassPhrase { get; set; }


        public string TeamConfirmationURL { get; set; }


        public virtual List<UserInRole> UserInRole { get; set; }

        public virtual List<MembershipTable> MembershipTables { get; set; }




        public UserProfile()
        {

        }

    }


}
