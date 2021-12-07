using ErpSolution.Application.Interfaces;
using ErpSolution.Application.Interfaces.UserManagement;
using ErpSolution.Application.Interfaces.UserMangement;
using ErpSolution.Infrastructure.Repositories;
using ErpSolution.Infrastructure.Repositories.UserManagement;
using MentokLibrary1_0;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ErpSolution.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            #region Repositories

            //services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
            //services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IUserClaimRepository, UserClaimRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPolicyDtlRepository, PolicyDtlRepository>();
            services.AddTransient<IPolicyMstRepository, PolicyMstRepository>();
            services.AddTransient<IModulesRepository, ModulesRepository>();
            services.AddTransient<IClaimsRepository, ClaimsRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMentok, MentokDB>(serviceProvider => new MentokDB(RDBMS.SqlServer, configuration.GetConnectionString("ApplicationConnection")));
            services.AddScoped(typeof(IBootStrapDataTableRepository<>), typeof(BootStrapDataTableRepository<>));
            services.AddScoped<IUserInformationRepository, UserInformationRepository>();
            #endregion Repositories
        }
    }
}
