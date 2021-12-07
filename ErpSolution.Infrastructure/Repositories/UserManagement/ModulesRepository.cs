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
    public class ModulesRepository : IModulesRepository
    {
        private IDbConnection db;
        private readonly IMentok mentok;

        public ModulesRepository(IConfiguration configuration,IMentok mentok)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("ApplicationConnection"));
            this.mentok = mentok;
        }

        public async Task<modules> AddAsync(modules module)
        {
           return  await mentok.CRUD.Create.InsertAutoKeyAsync<modules>(module);
        
            //var existing = (await db.QueryAsync<modules>("SELECT * FROM modules WHERE module_name = @module_name",
            //   new
            //   {
            //       @module_name = module.module_name
            //   })).FirstOrDefault();

            //if(existing == null)
            //{
            //    var sql = "INSERT INTO modules (module_name) VALUES (@module_name);" +
            //    "SELECT CAST(SCOPE_IDENTITY() AS int);";
            //    var id = (await db.QueryAsync<int>(sql, new
            //    {
            //        @module_name = module.module_name
            //    })).FirstOrDefault();
            //    module.module_id = id;
            //    return module;
            //}
            //else
            //{
            //    return null;
            //}
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM modules WHERE module_id = @Id";
            return await db.ExecuteAsync(sql, new { @Id = id });
        }

        public async Task<IQueryable<modules>> GetAllAsync()
        {
            var sql = "SELECT * FROM modules";
            return (await db.QueryAsync<modules>(sql)).AsQueryable();
        }

        public async Task<modules> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM modules WHERE module_id = @Id";
            return (await db.QueryAsync<modules>(sql, new { @Id = id })).FirstOrDefault();
        }

        public async Task<modules> UpdateAsync(modules module)
        {
            var current = (await db.QueryAsync<modules>("SELECT * FROM modules WHERE module_id = @module_id",
              new
              {
                  @module_id = module.module_id
              })).FirstOrDefault();

            var existing = (await db.QueryAsync<modules>("SELECT * FROM modules WHERE module_name = @module_name AND module_name != @current",
               new
               {
                   @module_name = module.module_name,
                   @current = current.module_name
               })).FirstOrDefault();

            if (existing == null)
            {
                var sql = "UPDATE modules SET module_name = @module_name WHERE module_id = @Id";
                await db.ExecuteAsync(sql, new
                {
                    @Id = module.module_id,
                    @module_name = module.module_name
                });
                return module;
            }
            else
            {
                return null;
            }
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
