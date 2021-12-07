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
    public class PolicyMstRepository : IPolicyMstRepository
    {
        private IDbConnection db;
        private readonly IMentok mentok;

        public PolicyMstRepository(IConfiguration configuration, IMentok mentok)
        {
            this.db = new SqlConnection(configuration.GetConnectionString("ApplicationConnection"));
            this.mentok = mentok;
        }

        public async Task<policy_mst> AddAsync(policy_mst policy)
        {
            return await mentok.CRUD.Create.InsertAutoKeyAsync<policy_mst>(policy);
            //var sql = "INSERT INTO policy_mst (policy_name, description) VALUES (@policy_name, @description);" +
            //"SELECT CAST(SCOPE_IDENTITY() AS int);";
            //var id = (await db.QueryAsync<int>(sql, new
            //{
            //    @policy_name = policy.policy_name,
            //    @description = policy.description
            //})).FirstOrDefault();
            //policy.policy_id = id;
            //return policy;
        }

        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                policy_mst model = new policy_mst { policy_id = id };
                await mentok.BeginTransactionAsync();
                await mentok.CRUD.Delete.DeleteAsync<policy_dtl>("policy_id=@Id", new { Id = id });
                var result = await mentok.CRUD.Delete.DeleteUsingKeyAsync<policy_mst>(model);
                await mentok.CommitTransactionAsync();
                return result;
            }
            catch (Exception ex)
            {
                await mentok.RollbackTransactionAsync();
                throw;
            }
            //await db.ExecuteAsync("DELETE FROM policy_dtl WHERE policy_id = @Id", new
            //{
            //    @Id = id
            //});

            //return await db.ExecuteAsync("DELETE FROM policy_mst WHERE policy_id = @Id", new
            //{
            //    @Id = id
            //});
            //using (var tran = db.BeginTransaction())
            //{
            //    try
            //    {
            //        await db.ExecuteAsync("DELETE FROM policy_dtl WHERE policy_id = @Id", new
            //        {
            //            @Id = id
            //        }, tran);

            //        await db.ExecuteAsync("DELETE FROM policy_mst WHERE policy_id = @Id", new
            //        {
            //            @Id = id
            //        }, tran);

            //        tran.Commit();
            //        return 1;
            //    }
            //    catch (Exception ex)
            //    {
            //        tran.Rollback();
            //        throw;
            //    }
            //}


        }

        public async Task<IQueryable<policy_mst>> GetAllAsync()
        {
            var sql = "SELECT * FROM policy_mst";
            return (await db.QueryAsync<policy_mst>(sql)).AsQueryable();
        }

        public async Task<policy_mst> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM policy_mst WHERE policy_id = @Id";
            return (await db.QueryAsync<policy_mst>(sql, new { @Id = id })).FirstOrDefault();
        }

        public async Task<policy_mst> UpdateAsync(policy_mst policy)
        {
            var sql = "UPDATE policy_mst SET policy_name = @policy_name, description = @description WHERE policy_id = @Id";
            await db.ExecuteAsync(sql, new
            {
                @Id = policy.policy_id,
                @policy_name = policy.policy_name,
                @description = policy.description
            });
            return policy;
        }
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
