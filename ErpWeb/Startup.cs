using ErpSolution.Application.Extensions;
using ErpSolution.Infrastructure.DbContexts;
using ErpSolution.Infrastructure.Extensions;
using ErpWeb.Extensions;
using ErpWeb.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ErpWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContextPool<IdentityContext>(options => options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));
            //services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<IdentityContext>();
            services.AddPersistenceContexts(Configuration);

            services.AddApplication();
            services.AddRepositories(Configuration);

            //services.AddAuthorization(options =>
            //{
            //    using (SqlConnection con = new SqlConnection(Configuration.GetConnectionString("IdentityConnection")))
            //    {
            //        using (SqlDataAdapter da = new SqlDataAdapter("select * from claims", con))
            //        {
            //            using (DataTable dtClaims = new DataTable())
            //            {
            //                da.Fill(dtClaims);

            //                foreach (DataRow row in dtClaims.Rows)
            //                {
            //                    options.AddPolicy(row["claim_name"].ToString(), policy => policy.RequireClaim(row["claim_name"].ToString()));
            //                }
            //            }
            //        }
            //    }
            //});

            services.AddAuthorization(Configuration);

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "UmbErp";
                options.ExpireTimeSpan = TimeSpan.FromSeconds(Convert.ToInt32(Configuration.GetSection("AppSettings")["ApplicationTimeOut"].ToString()));
                options.SlidingExpiration = true;
            });

            services.AddControllersWithViews();
            services.AddHttpContextAccessor();         

            services.AddAntiforgery(options =>
            {                
                options.Cookie.Expiration = TimeSpan.FromSeconds(Convert.ToInt32(Configuration.GetSection("AppSettings")["ApplicationTimeOut"].ToString()) + Convert.ToInt32(Configuration.GetSection("AppSettings")["ApplicationTimeOut"].ToString()));
                options.HeaderName = "X-ANTI-FORGERY-TOKEN";                
            });
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }           

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();           

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
