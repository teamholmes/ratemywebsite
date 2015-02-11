using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using OP.General.Extensions;
using System;

namespace ClaimsAuthorizeSample
{
    public class AuthorizationManager : ClaimsAuthorizationManager
    {
        public override bool CheckAccess(AuthorizationContext context)
        {

            var action = context.Action.First().Value.ToUpperCheckForNull();
            var resource = context.Resource;
            var userclaims = context.Principal.Claims.ToList();

            bool canAccessPage = false;

            var matchingkeys = userclaims.Where(x => x.Type.Equals(action, StringComparison.InvariantCultureIgnoreCase));

            if (!matchingkeys.Any()) throw new Exception("You do not have permissions to view this page");

            string uppercaseValue = matchingkeys.First().Value.ToUpperCheckForNull();

            var matchingvalues = resource.Where(x => x.Value.Equals(uppercaseValue, StringComparison.InvariantCultureIgnoreCase));

            if (!matchingvalues.Any()) throw new Exception("You do not have permissions to view this page");

            return true;





            //foreach (Claim cm in resource)
            //{
            //    foreach (Claim uclaims in userclaims)
            //    {
            //        if (action.ToLowerCheckForNull() == uclaims.Type.ToUpperCheckForNull()
            //            &&
            //            cm.Value.ToUpperCheckForNull() == uclaims.Value.ToUpperCheckForNull()
            //            )
            //        {
            //            canAccessPage = true;
            //            break;
            //        }

            //    }
            //}


            if (canAccessPage) return true;





            //  ClaimsPrincipal identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

            //Trace.WriteLine("\n\nClaimsAuthorizationManager\n_______________________\n");

            //Trace.WriteLine("\nAction:");
            //Trace.WriteLine("  " + context.Action.First().Value);

            //Trace.WriteLine("\nResources:");
            //foreach (var resource in context.Resource)
            //{
            //    Trace.WriteLine("  " + resource.Value);
            //}

            //Trace.WriteLine("\nClaims:");
            //foreach (var claim in context.Principal.Claims)
            //{
            //    Trace.WriteLine("  " + claim.Value);
            //}
        }
    }
}