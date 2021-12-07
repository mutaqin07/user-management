using ErpSolution.Application.Interfaces.UserManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace ErpSolution.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IClaimsRepository Claims { get; }
        IModulesRepository Modules { get; }
        IPolicyMstRepository PolicyMasters { get; }
        IPolicyDtlRepository PolicyDetails { get; }
    }
}
