using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErpWeb.Controllers
{
    public class ErrorController : Controller
    {
        [Authorize]
        [Route("Error/{statusCode}")]       
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            IStatusCodeReExecuteFeature statusCodeReExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 404:
                    ViewBag.StatusCode = "404";
                    ViewBag.OriginalPath = statusCodeReExecuteFeature?.OriginalPath;
                    ViewBag.QueryString = statusCodeReExecuteFeature?.OriginalQueryString;
                    ViewBag.ErrorMessage = "Maaf...URL tidak ditemukan";
                    break;
            }

            return View("NotFound");
        }

        [AllowAnonymous]
        [Route("Error")]
        public IActionResult Error()
        {
            IExceptionHandlerPathFeature exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            ViewBag.ExceptionPath = exceptionHandlerPathFeature.Path;
            ViewBag.ExceptionMessage = exceptionHandlerPathFeature.Error.Message;
            ViewBag.StackTrace = exceptionHandlerPathFeature.Error.StackTrace;

            return View("Error");
        }
    }
}
