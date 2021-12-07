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

namespace UserManagement.Controllers.Users
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ClaimController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        //[HttpPost]
        //[Authorize(Policy = "OPEN_CLAIMS")]
        //public async Task<IActionResult> GetDatatables([FromForm] DtParameters dtParameters)
        //{
        //    var searchBy = dtParameters.Search?.Value;

        //    // if we have an empty search then just order the results by Id ascending
        //    var orderCriteria = "claim_name";
        //    var orderAscendingDirection = true;

        //    if (dtParameters.Order != null)
        //    {
        //        // in this example we just default sort on the 1st column
        //        orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
        //        orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
        //    }

        //    var result = await unitOfWork.Claims.GetAllAsync();

        //    if (!string.IsNullOrEmpty(searchBy))
        //    {
        //        result = result.Where(r => r.claim_name != null && r.claim_name.ToUpper().Contains(searchBy.ToUpper()) ||
        //        r.description != null && r.description.ToUpper().Contains(searchBy.ToUpper()) ||
        //        r.module_id.ToString() != null && r.module_id.ToString().ToUpper().Contains(searchBy.ToUpper())
        //        );
        //    }

        //    result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);

        //    // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
        //    var filteredResultsCount = result.Count();
        //    var totalResultsCount = result.Count();

        //    var JsonData = (new DtResult<claims>
        //    {
        //        Draw = dtParameters.Draw,
        //        RecordsTotal = totalResultsCount,
        //        RecordsFiltered = filteredResultsCount,
        //        Data = result
        //            .Skip(dtParameters.Start)
        //            .Take(dtParameters.Length)
        //            .ToList()
        //    });
        //    return Ok(JsonData);
        //}

        [HttpPost]      
        [BootStrapDataTableCookiesValidation]
        public async Task<IActionResult> GetDatatables([FromServices] IBootStrapDataTableRepository<ErpSolution.Domain.Entities.BootStrapDataTable.claims> bootStrapDataTableRepository,[FromServices] IUserInformationRepository httpContextInformationRepository)
        {
            try
            {
                return await bootStrapDataTableRepository.GetData();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }          
        }

        [HttpPost]
        [ValidateAntiForgeryToken]       
        [IdentityValidation(Policy = "FIND_CLAIMS")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var data = await unitOfWork.Claims.GetByIdAsync(id);
            return Ok(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [IdentityValidation(Policy = "ADD_CLAIMS")]       
        public async Task<IActionResult> Add([FromForm] claims claim, [FromForm] string moduleName)
        {
            claim.claim_name = moduleName + "." + claim.claim_name;
            var data = await unitOfWork.Claims.AddAsync(claim);
            return Ok(new { MessageStatus = "OK", data = data });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [IdentityValidation(Policy = "DELETE_CLAIMS")]       
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            await unitOfWork.Claims.DeleteAsync(id);
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [IdentityValidation(Policy = "EDIT_CLAIMS")]       
        public async Task<IActionResult> Update([FromForm] claims claim)
        {
            //claim.claim_name = moduleName + "." + claim.claim_name;
            var data = await unitOfWork.Claims.UpdateAsync(claim);
            return Ok(new { MessageStatus = "OK", data = data });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [IdentityValidation(Policy = "ADD_CLAIMS")]
        public async Task<IActionResult> GetClaimsByModule(int? moduleId, string userId)
        {
            if (moduleId == 0 || moduleId == null)
            {
                return NotFound();
            }
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }
           
            var data = await unitOfWork.Claims.GetClaimsByModule(moduleId, userId);
            return Ok(data);
        }
    }
}
