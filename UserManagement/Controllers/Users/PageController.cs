using ErpSolution.Application.Interfaces.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErpSolution.Domain.Entities;

namespace UserManagement.Controllers.Users
{
    public class PageController : Controller
    {
        [Authorize(Policy = "OPEN_MODULES")]       
        public IActionResult Module()
        {
            return View();
        }

        [Authorize(Policy = "OPEN_CLAIMS")]
        public async Task<IActionResult> Claim([FromServices] IModulesRepository modules)
        {
            var getModules = await modules.GetAllAsync();
            ViewBag.Modules = (IEnumerable<modules>)getModules;
            return View();
        }

        [Authorize(Policy = "OPEN_POLICYH")]
        public async Task<IActionResult> Policy([FromServices] IPolicyMstRepository policy, [FromServices] IClaimsRepository claims)
        {
            var getPolicy = await policy.GetAllAsync();
            var getClaims = await claims.GetAllAsync();

            ViewBag.Policy = (IEnumerable<policy_mst>)getPolicy;
            ViewBag.Claims = (IEnumerable<claims>)getClaims;
            return View();
        }

        [Authorize(Policy = "OPEN_POLICYD")]
        public IActionResult DetailPolicy()
        {
            return View();
        }

        [Authorize(Policy = "OPEN_USERS")]
        public async Task<IActionResult> Users([FromServices] IModulesRepository module)
        {
            var getModules = await module.GetAllAsync();
            ViewBag.Modules = (IEnumerable<modules>)getModules;
            return View();
        }

        [Authorize(Policy = "OPEN_USERS_CLAIMS")]
        public IActionResult UserClaim()
        {
            return View();
        }
    }
}
