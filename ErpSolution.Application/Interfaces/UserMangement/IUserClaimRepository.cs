using ErpSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSolution.Application.Interfaces.UserMangement
{
    public interface IUserClaimRepository
    {
        Task<AspNetUserClaims> AddAsync(AspNetUserClaims aspNetUserClaims);
        Task<int> DeleteAsync(int id);
    }
}
