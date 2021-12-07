using ErpWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using ErpSolution.Application.Validations;
using ErpSolution.Domain.Entities;


namespace ErpWeb.Controllers
{
   
    public class MainMenuController : Controller
    {
        //[Authorize(Policy = "OPEN_EMPLOYEE")]
        //public async Task<IActionResult> Index([FromServices] IClaimsRepository claimsRepository)
        //{
        //    //var listClaims = await claimsRepository.GetClaims();
        //    return View();
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        [IdentityValidation(Policy = "ADD_EMPLOYEE")]
        public async Task<IActionResult> SubmitForm1([FromForm] Test test)
        {
            await Task.Delay(1000);            
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [IdentityValidation(Policy = "ADD_ASSET")]
        public async Task<IActionResult> SubmitForm2([FromForm] Test test)
        {
            await Task.Delay(1000);
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [IdentityValidation(Policy = "ADD_EMPLOYEE")]
        public async Task<IActionResult> SubmitForm3([FromForm] Test test)
        {           
            await Task.Delay(1000);          

            return Ok();
        }
    }
}