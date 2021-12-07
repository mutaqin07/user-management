using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ErpSolution.Application.DTOs.AuxiliaryModels;
using ErpSolution.Application.Extensions;
using ErpSolution.Application.Interfaces;
using ErpSolution.Application.Validations;
using ErpSolution.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErpSolution.Domain.Identity;
using ErpSolution.Application.Interfaces.UserMangement;

namespace UserManagement.Controllers.Users
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserClaimController : ControllerBase
    {
        [HttpPost]
        [BootStrapDataTableCookiesValidation]
        public async Task<IActionResult> GetDatatables([FromServices] IBootStrapDataTableRepository<ErpSolution.Domain.Entities.BootStrapDataTable.AspNetUserClaims> bootStrapDataTableRepository, [FromForm] string userId)
        {
            try
            {
                Dictionary<string, object> policyDetailParameter = new();

                policyDetailParameter.Add("@UserId", userId);
                return await bootStrapDataTableRepository.GetData(" UserId=@UserId ", policyDetailParameter);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
            //var searchBy = dtParameters.Search?.Value;

            //// if we have an empty search then just order the results by Id ascending
            //var orderCriteria = "UserId";
            //var orderAscendingDirection = true;

            //if (dtParameters.Order != null)
            //{
            //    // in this example we just default sort on the 1st column
            //    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
            //    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            //}

            //var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            //var result = (await _userManager.Users.Where(a => a.Id != currentUser.Id).ToListAsync()).AsQueryable();

            //if (!string.IsNullOrEmpty(searchBy))
            //{
            //    result = result.Where(r => r.UserName != null && r.UserName.ToUpper().Contains(searchBy.ToUpper()) ||
            //    r.Email != null && r.Email.ToUpper().Contains(searchBy.ToUpper()) ||
            //    r.PhoneNumber != null && r.PhoneNumber.ToUpper().Contains(searchBy.ToUpper())
            //    );
            //}

            //result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);

            //// now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            //var filteredResultsCount = result.Count();
            //var totalResultsCount = result.Count();

            //var JsonData = (new DtResult<IdentityUser>
            //{
            //    Draw = dtParameters.Draw,
            //    RecordsTotal = totalResultsCount,
            //    RecordsFiltered = filteredResultsCount,
            //    Data = result
            //        .Skip(dtParameters.Start)
            //        .Take(dtParameters.Length)
            //        .ToList()
            //});
            //return Ok(JsonData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [IdentityValidation(Policy = "ADD_USER_CLAIMS")]
        public async Task<IActionResult> Add([FromForm] AspNetUserClaims module, [FromServices] IUserClaimRepository _userRepository)
        {
            try
            {
                module = await _userRepository.AddAsync(module);
                return Ok(new { MessageStatus = "OK", data = module });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [IdentityValidation(Policy = "DELETE_USER_CLAIMS")]
        public async Task<IActionResult> Delete([FromServices] IUserClaimRepository _userRepository, int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            await _userRepository.DeleteAsync(id);
            return Ok();
        }

    }
}
