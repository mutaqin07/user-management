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

namespace ErpSolution.Infrastructure.Repositories.UserManagement
{
    public class UserRepository : IUserRepository
    {
        private readonly IMentok mentok;
        private IDbConnection db;

        public UserRepository(IMentok mentok, IConfiguration configuration)
        {
            this.mentok = mentok;
            this.db = new SqlConnection(configuration.GetConnectionString("ApplicationConnection"));
        }

        public async Task<AspNetUsers> UpdateAsync(AspNetUsers model)
        {
            var sql = "UPDATE AspNetUsers SET FullName=@FullName, Email=@Email, NormalizedEmail=@NormalizedEmail, PhoneNumber=@PhoneNumber " +
                "WHERE Id = @Id";
            await db.ExecuteAsync(sql, new
            {
                @FullName = model.FullName,
                @Email = model.Email,
                @NormalizedEmail = model.Email.ToUpper(),
                @PhoneNumber = model.PhoneNumber,
                @Id = model.Id
            });
            return model;
        }

        public async Task<int> ChangeStatusAsync(string id)
        {
            var current = (await db.QueryAsync<AspNetUsers>("SELECT * FROM AspNetUsers WHERE Id = @Id",
              new
              {
                  Id = id
              })).FirstOrDefault();
            var statusActive = (current.IsActive == true) ? false : true;
            var sql = "UPDATE AspNetUsers SET IsActive=@IsActive WHERE Id = @Id";
            var result = await db.ExecuteAsync(sql, new
            {
                @IsActive = statusActive,
                @Id = id
            });
            return result;
        }
    }
}
