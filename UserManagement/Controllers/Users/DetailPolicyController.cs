using ErpSolution.Application.Interfaces;
using ErpSolution.Application.Validations;
using ErpSolution.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserManagement.Controllers.Users
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DetailPolicyController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public DetailPolicyController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]       
        [BootStrapDataTableCookiesValidation]
        public async Task<IActionResult> GetDatatables([FromServices] IBootStrapDataTableRepository<ErpSolution.Domain.Entities.BootStrapDataTable.policy_dtl> bootStrapDataTableRepository, [FromForm] int? id)
        {
            try
            {
                Dictionary<string, object> policyDetailParameter = new();

                policyDetailParameter.Add("@policy_id_new", id);
                return await bootStrapDataTableRepository.GetData(" policy_id=@policy_id_new ", policyDetailParameter);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        //[HttpPost]
        //[Authorize(Policy = "OPEN_POLICYD")]
        //public async Task<IActionResult> GetDatatables([FromForm] DtParameters dtParameters)
        //{
        //    var searchBy = dtParameters.Search?.Value;

        //    // if we have an empty search then just order the results by Id ascending
        //    var orderCriteria = "id";
        //    var orderAscendingDirection = true;

        //    if (dtParameters.Order != null)
        //    {
        //        // in this example we just default sort on the 1st column
        //        orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
        //        orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
        //    }

        //    var result = await unitOfWork.PolicyDetails.GetAllAsync();

        //    if (!string.IsNullOrEmpty(searchBy))
        //    {
        //        result = result.Where(r => r.claim_name != null && r.claim_name.ToUpper().Contains(searchBy.ToUpper()) ||
        //        r.policy_id.ToString() != null && r.policy_id.ToString().ToUpper().Contains(searchBy.ToUpper())
        //        );
        //    }

        //    result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);

        //    var filteredResultsCount = result.Count();
        //    var totalResultsCount = result.Count();

        //    var JsonData = (new DtResult<policy_dtl>
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

        //[HttpPost]
        //[ValidateAntiForgeryToken]       
        //[IdentityValidation(Policy = "FIND_POLICYD")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    if (id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var data = await unitOfWork.PolicyDetails.GetByIdAsync(id);
        //    return Ok(data);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        [IdentityValidation(Policy = "ADD_POLICYD")]
       
        public async Task<IActionResult> Add([FromForm] policy_dtl policy)
        {
            try
            {
                var data = await unitOfWork.PolicyDetails.AddAsync(policy);
                return Ok(new { MessageStatus = "OK", data = data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [IdentityValidation(Policy = "DELETE_POLICYD")]       
        public async Task<IActionResult> Delete(string policyId, string claimName)
        {
            if (string.IsNullOrEmpty(policyId) || string.IsNullOrEmpty(claimName))
            {
                return NotFound();
            }
            await unitOfWork.PolicyDetails.DeleteByParamAsync(policyId, claimName);
            return Ok();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[IdentityValidation(Policy = "EDIT_POLICYD")]       
        //public async Task<IActionResult> Update([FromForm] policy_dtl policy)
        //{
        //    try
        //    {
        //        var data = await unitOfWork.PolicyDetails.UpdateAsync(policy);
        //        return Ok(new { MessageStatus = "OK", data = data });

        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500);
        //    }
        //}
    }
}
