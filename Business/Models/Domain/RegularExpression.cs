using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OP.General;

namespace MyApp.Business.DomainObjects.Models
{
    public static class RegularExpression 
    {

        public const string EmailAddressRegex = @"^[A-Za-z0-9,!#\$%&'\*\+/=\?\^_`\{\|}~-]+(\.[A-Za-z0-9,!#\$%&'\*\+/=\?\^_`\{\|}~-]+)*@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)*\.([a-z]{2,})$";


        //It will match numbers like: 0123456789, 012-345-6789, (012)-345-6789 etc.
        public const string PhoneNumberRegex = @"^[0-9 -.]*$";

       


    }


     
}
