using Crayon.Domain.Consts;
using Crayon.Domain.Entities;

using Microsoft.AspNetCore.Identity;

namespace Crayon.Persistence
{
    public static class Seed
    {
        public static async Task SeedDataAsync(DataContext context, UserManager<User> userManager)
        {
            if (!userManager.Users.Any())
            {
                var customers = new List<Customer>
                {
                    new Customer
                    {
                        CompanyName = "ACME",
                        ContactInfo = "acme@test.com",
                        CustomerType = CustomerType.Direct
                    }
                };

                context.Customers.AddRange(customers);

                var accounts = new List<Account>
                {
                    new Account
                    {
                        Customer = customers[0],
                        Name = "ACME-TEST-ACCOUNT",
                        Description = "Test account"
                    }
                };

                context.Accounts.AddRange(accounts);

                await context.SaveChangesAsync();

                var users = new List<User>()
                {
                    new User
                    {
                        FirstName = "Nemanja",
                        LastName = "Djordjevic",
                        UserName = "nemanja",
                        Email = "nemanja@test.com",
                        Customer = customers[0]
                    }
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, password: "NemanjaDjordjevic@1994");
                }
            }
        }
    }
}
