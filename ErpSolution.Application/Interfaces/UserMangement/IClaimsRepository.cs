using ErpSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSolution.Application.Interfaces.UserManagement
{
    public interface IClaimsRepository : IGenericStringIdRepository<claims>
    {
        //Task<IEnumerable<claims>> GetClaims();
        Task<IEnumerable<claims>> GetClaimsByModule(int? moduleId, string userId);
    }
}
