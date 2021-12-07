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
    public class PolicyController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public PolicyController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        [BootStrapDataTableCookiesValidation]
        public async Task<IActionResult> GetDatatables([FromServices] IBootStrapDataTableRepository<ErpSolution.Domain.Entities.BootStrapDataTable.policy_mst> bootStrapDataTableRepository)
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

        //[HttpPost]
        //[Authorize(Policy = "OPEN_POLICYH")]
        //public async Task<IActionResult> GetDatatables([FromForm] DtParameters dtParameters)
        //{
        //    var searchBy = dtParameters.Search?.Value;

        //    // if we have an empty search then just order the results by Id ascending
        //    var orderCriteria = "policy_id";
        //    var orderAscendingDirection = true;

        //    if (dtParameters.Order != null)
        //    {
        //        // in this example we just default sort on the 1st column
        //        orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
        //        orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
        //    }

        //    var result = await unitOfWork.PolicyMasters.GetAllAsync();

        //    if (!string.IsNullOrEmpty(searchBy))
        //    {
        //        result = result.Where(r => r.policy_name != null && r.policy_name.ToUpper().Contains(searchBy.ToUpper()) ||
        //        r.description != null && r.description.ToUpper().Contains(searchBy.ToUpper()) ||
        //        r.policy_id.ToString() != null && r.policy_id.ToString().ToUpper().Contains(searchBy.ToUpper())
        //        );
        //    }

        //    result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);

        //    var filteredResultsCount = result.Count();
        //    var totalResultsCount = result.Count();

        //    var JsonData = (new DtResult<policy_mst>
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
        [ValidateAntiForgeryToken]      
        [IdentityValidation(Policy = "FIND_POLICYH")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var data = await unitOfWork.PolicyMasters.GetByIdAsync(id);
            return Ok(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [IdentityValidation(Policy = "ADD_POLICYH")]      
        public async Task<IActionResult> Add([FromForm] policy_mst policy)
        {
            try
            {
                var data = await unitOfWork.PolicyMasters.AddAsync(policy);
                return Ok(new { MessageStatus = "OK", data = data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [IdentityValidation(Policy = "DELETE_POLICYH")]       
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            await unitOfWork.PolicyMasters.DeleteAsync(id);
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [IdentityValidation(Policy = "EDIT_POLICYH")]      
        public async Task<IActionResult> Update([FromForm] policy_mst policy)
        {
            try
            {
                var data = await unitOfWork.PolicyMasters.UpdateAsync(policy);
                return Ok(new { MessageStatus = "OK", data = data });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
