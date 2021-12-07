using ErpSolution.Application.Interfaces.UserManagement;
using Microsoft.Extensions.Configuration;
using ErpSolution.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MentokLibrary1_0;
using ErpSolution.Domain.Entities;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using ErpSolution.Application.Interfaces.UserMangement;

namespace ErpSolution.Infrastructure.Repositories.UserManagement
{
    public class UserClaimRepository : IUserClaimRepository
    {
        private IDbConnection db;

        public UserClaimRepository (IConfiguration configuration)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("ApplicationConnection"));
        }

        public async Task<AspNetUserClaims> AddAsync(AspNetUserClaims aspNetUserClaims)
        {
            var sql = "INSERT INTO AspNetUserClaims (UserId, ClaimType, ClaimValue) VALUES (@UserId, @ClaimType, @ClaimValue)";
            await db.ExecuteAsync(sql, new
            {
                @UserId = aspNetUserClaims.UserId,
                @ClaimType = aspNetUserClaims.ClaimType,
                @ClaimValue = aspNetUserClaims.ClaimValue
            });

            return aspNetUserClaims;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM AspNetUserClaims WHERE Id = @Id";
            return await db.ExecuteAsync(sql, new { @Id = id });
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
