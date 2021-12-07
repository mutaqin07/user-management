using ErpSolution.Application.Interfaces;
using ErpSolution.Domain.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ErpSolution.Infrastructure.Repositories
{
    public class UserInformationRepository : IUserInformationRepository
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;

        public UserInformationRepository(IHttpContextAccessor httpContextAccessor,UserManager<ApplicationUser> userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public async Task<string> NIK()
        {
            ApplicationUser applicationUser = await   userManager.GetUserAsync(httpContextAccessor.HttpContext.User);

            return applicationUser.UserName;
        }

        public async Task<string> FullName()
        {
            ApplicationUser applicationUser = await userManager.GetUserAsync(httpContextAccessor.HttpContext.User);

            return applicationUser.FullName;
        }
    }
}
