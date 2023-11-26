namespace Crayon.Domain.Entities
{
    public class Customer : Entity
    {
        public string CompanyName { get; set; }

        public string ContactInfo { get; set; }

        public string CustomerType { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
