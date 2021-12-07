using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ErpSolution.Application.Validations
{
    public class IdentityValidation : Attribute, IAsyncAuthorizationFilter
    {

        public string Policy { get; set; }


        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {

                Claim claim = context.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == Policy);

                if (claim == null)
                {
                    context.Result = new UnauthorizedObjectResult(new { MessageStatus = "NoAccessRight" });
                }
                else
                {
                    try
                    {
                        IAntiforgery antiforgery = (IAntiforgery)context.HttpContext.RequestServices.GetService(typeof(IAntiforgery));

                        await antiforgery.ValidateRequestAsync(context.HttpContext);
                       
                    }
                    catch (AntiforgeryValidationException)
                    {
                        context.Result = new BadRequestObjectResult(new { MessageStatus = "AntiForgeryTokenExpired" }); ;
                    }

                }
            }
            else
            {
                context.Result = new UnauthorizedObjectResult(new { MessageStatus = "CookiesExpired" });
            }
        }
    }
}
