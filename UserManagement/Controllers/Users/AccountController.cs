using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using ErpSolution.Application.Responses;
using ErpSolution.Application.DTOs.Identity;
using ErpSolution.Domain.Identity;

namespace UserManagement.Controllers.Users
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]       
        //[IgnoreAntiforgeryToken]
        public async Task<IActionResult> Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }

            //if (HttpContext.Request.Cookies["UmbAntiForgeryTokenCookie"] != null)
            //{
                HttpContext.Response.Cookies.Delete("UmbAntiForgeryTokenCookie");             
            //}

            //if (HttpContext.Request.Cookies["UmbErp"] != null)
            //{
                HttpContext.Response.Cookies.Delete("UmbErp");
            //}

            
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel loginViewModel)
        {
            try
            {
                var userName = loginViewModel.UserName;
                var user = await _userManager.FindByNameAsync(userName);
                if(user != null)
                {
                    userName = user.UserName;
                    if (!user.IsActive)
                    {
                        return Json(new { status = "Deactivated" });
                    }
                    else
                    {
                        var signInResult = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, false, false);

                        if (signInResult.Succeeded)
                        {
                            return Json(new { status = "OK", returnUrl = loginViewModel.ReturnUrl });
                        }
                        else
                        {
                            return Json(new { status = "UnAuthorize" });
                        }
                    }
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
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Deactivated()
        {
            return View();
        }
    }
}
