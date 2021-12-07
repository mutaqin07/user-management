using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
//using ModelProvider.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using GeneralModule;
using System.Diagnostics;
using ErpSolution.Application.Responses;
using ErpSolution.Application.DTOs.Identity;

namespace ErpWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await signInManager.SignOutAsync();
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel loginViewModel)
        {
            try
            {
                var signInResult = await signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, false, false);

                if (signInResult.Succeeded)
                {
                    return Json(new { status = "OK", returnUrl = loginViewModel.ReturnUrl });
                }
                else
                {
                    return Json(new { status = "UnAuthorize" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = ex.Message });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }
    }
}
