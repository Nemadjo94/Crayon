namespace Crayon.Domain.Entities
{
    public class Account : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Customer Customer { get; set; }

        public ICollection<AccountService> Services { get; set; }
    }
}
