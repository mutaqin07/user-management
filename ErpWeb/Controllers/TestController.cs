using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ErpWeb.Controllers
{
    public class TestController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        public TestController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            IdentityUser identityUser = new IdentityUser() { UserName = "wisnu", Email = "wisnu.ulandaru@gmail.com" };
            
            IdentityResult identityResult = await userManager.CreateAsync(identityUser, "Wisnugagah7787!");
           
            return View();
        }

        public async Task<IActionResult> CreateClaims()
        {
           IdentityUser identityUser = await userManager.FindByNameAsync("wisnu");

            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim("OPEN_EMPLOYEE", "OPEN_EMPLOYEE"));
            claims.Add(new Claim("ADD_EMPLOYEE", "ADD_EMPLOYEE"));

           IdentityResult identityResult = await  userManager.AddClaimsAsync(identityUser, claims);

            return View();
        }
    }
}
