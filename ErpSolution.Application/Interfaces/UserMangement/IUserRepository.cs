using ErpSolution.Domain.Entities;
using ErpSolution.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSolution.Application.Interfaces.UserManagement
{
    public interface IUserRepository
    {
        Task<AspNetUsers> UpdateAsync(AspNetUsers model);
        Task<int> ChangeStatusAsync(string id);
    }
}
