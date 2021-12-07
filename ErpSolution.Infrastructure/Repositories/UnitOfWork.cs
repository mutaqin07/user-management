using ErpSolution.Application.Interfaces;
using ErpSolution.Application.Interfaces.UserManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace ErpSolution.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IClaimsRepository Claims { get; }
        public IModulesRepository Modules { get; }
        public IPolicyMstRepository PolicyMasters { get; }
        public IPolicyDtlRepository PolicyDetails { get; }

        public UnitOfWork(
            IClaimsRepository claimRepo,
            IModulesRepository modulesRepo,
            IPolicyMstRepository policyMstRepo,
            IPolicyDtlRepository policyDtlRepo
            )
        {
            Claims = claimRepo;
            Modules = modulesRepo;
            PolicyMasters = policyMstRepo;
            PolicyDetails = policyDtlRepo;
        }
    }
}
