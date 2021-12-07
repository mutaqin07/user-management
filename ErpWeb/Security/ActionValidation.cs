using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErpWeb.Security
{
    public class ActionValidation : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.Response.StatusCode == 200) 
            {
                context.Result = new OkObjectResult(new { Status = "OK", NewAntiForgeryToken = ((IAntiforgery)context.HttpContext.RequestServices.GetService(typeof(IAntiforgery))).GetAndStoreTokens(context.HttpContext).RequestToken });
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
