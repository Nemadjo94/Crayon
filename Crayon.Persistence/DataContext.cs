using Crayon.Application.Interfaces;
using Crayon.Domain.Entities;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Crayon.Persistence
{
    public sealed class DataContext : IdentityDbContext<User>, IDataContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
                
        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<AccountService> AccountServices { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AccountService>()
                .HasKey(e => new { e.AccountId, e.ServiceId });

            base.OnModelCreating(builder);
        }
    }
}
