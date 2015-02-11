using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OP.General;

namespace MyApp.Business.DomainObjects.Models
{
    public class UserInRole
    {

        public int Id { get; set; }

        public int UserId { get; set; }

        public int RoleId { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public UserInRole()
        {

        }

    }



}
