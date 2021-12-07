using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using ErpSolution.Infrastructure.DbContexts;
using ErpSolution.Domain.Identity;

namespace UserManagement.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPersistenceContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityContext>(options => options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ApplicationConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<IdentityContext>();
        }

        public static void AddAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization(options =>
            {
                using (SqlConnection con = new SqlConnection(configuration.GetConnectionString("ApplicationConnection")))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter("select * from claims", con))
                    {
                        using (DataTable dtClaims = new DataTable())
                        {
                            da.Fill(dtClaims);

                            foreach (DataRow row in dtClaims.Rows)
                            {
                                options.AddPolicy(row["claim_name"].ToString(), policy => policy.RequireClaim(row["claim_name"].ToString()));
                            }
                        }
                    }
                }
            });
        }
    }
}
