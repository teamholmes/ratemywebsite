using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using OP.General;

namespace MyApp.Business.DomainObjects.Models
{
    [Serializable]
    public class SessionModel 
    {
        public int UserProfileId { get; set; }

        public int TeamCountryId { get; set; }

        public string EmailAddress { get; set; }

        public string Username { get; set; }

        public int Phase1RegistrationFormId  { get; set; }

        public int Phase2RegistrationFormId { get; set; }

        public int Phase3RegistrationFormId { get; set; }

        public string RolesUserInAsPipeSeparated { get; set; }

        public Boolean IsTeam { get; set; }

        public string EncryptedEventTeamIdBeingReviewdByAdmin { get; set; }

        public SessionModel()
        {

        }

    }


     
}
