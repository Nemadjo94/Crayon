namespace Crayon.Domain.Entities
{
    public class Service : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<AccountService> Accounts { get; set; }
    }
}
