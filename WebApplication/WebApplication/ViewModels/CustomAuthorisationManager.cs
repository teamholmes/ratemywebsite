using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OP.General.Extensions;

//http://dotnetcodr.com/2013/02/21/introduction-to-claims-based-security-in-net4-5-with-c-part-4-authorisation-with-claims/


//https://simplesecurity.codeplex.com/SourceControl/latest#SimpleSecurityMVC5/SimpleSecurity.Filters/SimpleSecurity.Filters.csproj

namespace System.Security.Claims
{

    public class CustomClaims
    {
        public List<CustomClaim> ClaimsRequired { get; set; }

        public CustomClaims(List<CustomClaim> claims)
        {
            ClaimsRequired = new List<CustomClaim>();
            ClaimsRequired = claims;

        }

    }

    public class CustomClaim : Dictionary<string, string>
    {

        public string Key { get; set; }
        public string Value { get; set; }


        public CustomClaim(string key, string value)
        {
            Key = key;
            Value = value;
        }

    }

    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class SimpleAuthorize : AuthorizeAttribute
    {
        //public SimpleAuthorize(int ResourceId, Operations operation)

        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }




        public SimpleAuthorize()
        {
        }

        public SimpleAuthorize(string claimType, string claimValue)
        {
            //    _operation = operation;
            ClaimType = claimType;
            ClaimValue = claimValue;

        }


        // private Operations _operation;

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var prinicpal = (ClaimsPrincipal)Thread.CurrentPrincipal;

            if (!prinicpal.Identity.IsAuthenticated)
            {
                throw new Exception("You are not authorised to access this page");
            }
            else
            {
                ClaimType = ClaimType.ToUpperCheckForNull();
                ClaimValue = ClaimValue.ToUpperCheckForNull();

                if (ClaimType.IsNullOrEmpty() || ClaimValue.IsNullOrEmpty()) return true;

                string[] arrayofClaimValues = ClaimValue.SplitOnPipe();

                Boolean matchfound = false;
                for (int counter = 0; counter < arrayofClaimValues.Length; counter++)
                {
                    string tofind = arrayofClaimValues[counter];

                    var result = prinicpal.Claims.Where(x => x.Type.Equals(ClaimType, StringComparison.InvariantCultureIgnoreCase) && x.Value.Equals(tofind, StringComparison.InvariantCultureIgnoreCase));
                    if (result.Any())
                    {
                        matchfound = true;
                        break;
                    }
                }

                if (matchfound) return true;

                throw new Exception("You do not have permission to view this page");
            }
            return true;
        }

    }


}