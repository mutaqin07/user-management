using System;
using System.Collections.Generic;
using System.Text;
using ErpSolution.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ErpSolution.Infrastructure.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<claims> Claims { get; set; }
        public DbSet<modules> Modules { get; set; }
        public DbSet<policy_mst> MasterPolicy { get; set; }
        public DbSet<policy_dtl> DetailPolicy { get; set; }
    }
}
