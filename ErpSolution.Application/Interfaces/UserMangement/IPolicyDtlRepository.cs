using ErpSolution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSolution.Application.Interfaces.UserManagement
{
    public interface IPolicyDtlRepository : IGenericStringIdRepository<policy_dtl>
    {
        Task<int> DeleteByParamAsync(string policyId, string claimName);
    }
}
