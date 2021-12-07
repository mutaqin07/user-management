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
    public class ClaimsRepository : IClaimsRepository
    {
        private IDbConnection db;

        public ClaimsRepository(IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("ApplicationConnection"));
        }

        public async Task<claims> AddAsync(claims claim)
        {
            var sql = "INSERT INTO claims (claim_name, module_id, description) VALUES (@claim_name, @module_id, @description)";
            await db.ExecuteAsync(sql, new
            {
                @claim_name = claim.claim_name,
                @module_id = claim.module_id,
                @description = claim.description
            });

            return claim;
        }

        public async Task<int> DeleteAsync(string id)
        {
            var sql = "DELETE FROM claims WHERE claim_name = @Id";
            return await db.ExecuteAsync(sql, new { @Id = id });
        }

        public async Task<IQueryable<claims>> GetAllAsync()
        {
            var sql = "SELECT * FROM claims";
            return (await db.QueryAsync<claims>(sql)).AsQueryable();
        }

        public async Task<claims> GetByIdAsync(string id)
        {
            var sql = "SELECT * FROM claims WHERE claim_name = @Id";
            return (await db.QueryAsync<claims>(sql, new { @Id = id })).FirstOrDefault();
        }

        public async Task<claims> UpdateAsync(claims claim)
        {
            var sql = "UPDATE claims SET module_id = @module_id, description = @description WHERE claim_name = @Id";
            await db.ExecuteAsync(sql, new
            {
                @Id = claim.claim_name,
                @module_id = claim.module_id,
                @description = claim.description
            });
            return claim;
        }

        public async Task<IEnumerable<claims>> GetClaimsByModule(int? moduleId, string userId)
        {
            var sql = "SELECT * FROM claims WHERE module_id = @module_id AND claim_name NOT IN (SELECT ClaimType FROM AspNetUserClaims WHERE UserId = @UserId)";
            return await db.QueryAsync<claims>(sql, new 
            { 
                @module_id = moduleId,
                @UserId = userId
            });
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
