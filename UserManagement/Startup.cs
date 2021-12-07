using ErpSolution.Application.Extensions;
using ErpSolution.Infrastructure.DbContexts;
using ErpSolution.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using UserManagement.Extensions;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;

namespace UserManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPersistenceContexts(Configuration);
            //services.AddNotyf(o =>
            //{
            //    o.DurationInSeconds = 10;
            //    o.IsDismissable = true;
            //    o.HasRippleEffect = true;
            //});
            services.AddApplication();
            services.AddRepositories(Configuration);

            services.AddAuthorization(Configuration);

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "UmbErp";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(Convert.ToInt32(Configuration.GetSection("AppSettings")["ApplicationTimeOut"].ToString()));
                options.SlidingExpiration = true;               
            });

            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddJsonOptions(configure => configure.JsonSerializerOptions.PropertyNamingPolicy = null);
            services.AddHttpContextAccessor();

            services.AddAntiforgery(options =>  {
                options.HeaderName = "X-ANTI-FORGERY-TOKEN";
                options.Cookie.Name = "UmbAntiForgeryTokenCookie";
                options.Cookie.MaxAge = TimeSpan.FromMinutes(Convert.ToInt32(Configuration.GetSection("AppSettings")["ApplicationTimeOut"].ToString()) + (Convert.ToInt32(Configuration.GetSection("AppSettings")["ApplicationTimeOut"].ToString())));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                   pattern: "{controller=Dashboard}/{action=Index}/{id?}");
                endpoints.MapControllers();
            });
        }
    }
}
