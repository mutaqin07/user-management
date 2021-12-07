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
using System.Linq.Dynamic.Core;
using ErpSolution.Application.Interfaces.UserManagement;

namespace UserManagement.Controllers.Users
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public ModuleController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        //    if (dtParameters.Order != null)
        //    {
        //        // in this example we just default sort on the 1st column
        //        orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
        //        orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
        //    }

        //    var result = await unitOfWork.Modules.GetAllAsync();

        //    if (!string.IsNullOrEmpty(searchBy))
        //    {
        //        result = result.Where(r => r.module_name != null && r.module_name.ToUpper().Contains(searchBy.ToUpper()));
        //    }

        //    result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);

        //    // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
        //    var filteredResultsCount = result.Count();
        //    var totalResultsCount = result.Count();

        //    var JsonData = (new DtResult<modules>
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
        public async Task<IActionResult> GetDatatables([FromServices] IBootStrapDataTableRepository<ErpSolution.Domain.Entities.BootStrapDataTable.modules> bootStrapDataTableRepository)
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
        [IdentityValidation(Policy = "FIND_MODULES")]

        public async Task<IActionResult> GetById(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var data = await unitOfWork.Modules.GetByIdAsync(id);
            return Ok(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [IdentityValidation(Policy = "ADD_MODULES")]
        public async Task<IActionResult> Add([FromForm]modules module, [FromServices] IModulesRepository modulesRepository)
        {
            try
            {
                module = await modulesRepository.AddAsync(module);
                return Ok( new { MessageStatus ="OK", data = module } );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [IdentityValidation(Policy = "DELETE_MODULES")]

        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            await unitOfWork.Modules.DeleteAsync(id);
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [IdentityValidation(Policy = "EDIT_MODULES")]

        public async Task<IActionResult> Update([FromForm] modules module)
        {
            try
            {
                module = await unitOfWork.Modules.UpdateAsync(module);
                return Ok(new { MessageStatus = "OK", data = module });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
