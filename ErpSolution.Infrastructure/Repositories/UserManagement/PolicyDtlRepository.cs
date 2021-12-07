using Dapper;
using ErpSolution.Application.Interfaces.UserManagement;
using ErpSolution.Domain.Entities;
using MentokLibrary1_0;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSolution.Infrastructure.Repositories.UserManagement
{
    public class PolicyDtlRepository : IPolicyDtlRepository
    {
        private IDbConnection db;
        private readonly IMentok mentok;

        public PolicyDtlRepository(IConfiguration configuration, IMentok mentok)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("ApplicationConnection"));
            this.mentok = mentok;
        }

        public async Task<policy_dtl> AddAsync(policy_dtl policy)
        {
            //return await mentok.CRUD.Create.InsertAsync<policy_dtl>(policy);
            var sql = "INSERT INTO policy_dtl (policy_id, claim_name) VALUES (@policy_id, @claim_name)";
            await db.ExecuteAsync(sql, new
            {
                @policy_id = policy.policy_id,
                @claim_name = policy.claim_name
            });
            return policy;
        }

        public async Task<int> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteByParamAsync(string policyId, string claimName)
        {
            var sql = "DELETE FROM policy_dtl WHERE policy_id = @policy_id AND claim_name = @claim_name";
            return await db.ExecuteAsync(sql, new
            {
                @policy_id = policyId,
                @claim_name = claimName
            });
        }

        public async Task<IQueryable<policy_dtl>> GetAllAsync()
        {
            var sql = "SELECT * FROM policy_dtl";
            return (await db.QueryAsync<policy_dtl>(sql)).AsQueryable();
        }

        public Task<policy_dtl> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<policy_dtl> UpdateAsync(policy_dtl entity)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            db.Dispose();
        }

       
    }
}
