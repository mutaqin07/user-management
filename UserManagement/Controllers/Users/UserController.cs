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
using ErpSolution.Application.DTOs.Identity;
using System.Text;
using ErpSolution.Application.Interfaces.UserManagement;

namespace UserManagement.Controllers.Users
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;

        public UserController(UserManager<ApplicationUser> userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        [HttpPost]
        [BootStrapDataTableCookiesValidation]
        public async Task<IActionResult> GetDatatables([FromServices] IBootStrapDataTableRepository<ErpSolution.Domain.Entities.BootStrapDataTable.AspNetUsers> bootStrapDataTableRepository)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                Dictionary<string, object> parameter = new();

                parameter.Add("@Id", currentUser.Id);
                
                return await bootStrapDataTableRepository.GetData(" Id != @Id", parameter);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [IdentityValidation(Policy = "ADD_USERS")]
        public async Task<IActionResult> Add([FromForm] RegisterModel model)
        {
            try
            {
                var sb = new StringBuilder();
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FullName = model.FullName,
                    EmailConfirmed = true,
                    PhoneNumber = model.PhoneNumber,
                    IsActive = true
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return Ok(new { MessageStatus = "OK", data = user });
                }
                foreach (var error in result.Errors)
                {
                   sb.AppendLine(error.Description);
                }
                return StatusCode(500, new { Message = sb.ToString() });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [IdentityValidation(Policy = "FIND_USERS")]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            return Ok(await _userManager.FindByIdAsync(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [IdentityValidation(Policy = "EDIT_USERS")]
        public async Task<IActionResult> Update([FromServices] IUserRepository userRepo, [FromForm] AspNetUsers model)
        {
            try
            {
                //AspNetUsers user = new AspNetUsers
                //{
                //    Id = model.Id,
                //    FullName = model.Email,
                //    Email = model.Email,
                //    PhoneNumber = model.PhoneNumber
                //};
                model = await userRepo.UpdateAsync(model);
                return Ok(new { MessageStatus = "OK", data = model });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [IdentityValidation(Policy = "EDIT_USERS")]
        public async Task<IActionResult> ChangeStatus([FromServices] IUserRepository userRepo, [FromForm] string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return NotFound();
                }

                await userRepo.ChangeStatusAsync(id);
                return Ok(new { MessageStatus = "OK" });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
