using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ErpWeb.Security
{
    public class IdentityValidation : Attribute, IAuthorizationFilter
    { 
        public string Policy { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                Claim claim = context.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == Policy);

                if (claim == null)
                {                  
                   context.Result = new UnauthorizedObjectResult(new { Message = "NoAccessRight", NewAntiForgeryToken = ((IAntiforgery)context.HttpContext.RequestServices.GetService(typeof(IAntiforgery))).GetAndStoreTokens(context.HttpContext).RequestToken });
                }
            }
            else
            {
                context.Result = new UnauthorizedObjectResult(new { Message = "CookiesExpired" });
            }
        }
    }
}
