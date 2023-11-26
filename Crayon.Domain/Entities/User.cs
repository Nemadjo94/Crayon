using Microsoft.AspNetCore.Identity;

namespace Crayon.Domain.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Customer Customer { get; set; }
    }
}
