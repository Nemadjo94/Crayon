using Crayon.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Crayon.Application.Interfaces
{
    public interface IDataContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<AccountService> AccountServices { get; set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
