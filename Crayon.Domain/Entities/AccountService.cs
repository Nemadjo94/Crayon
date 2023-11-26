namespace Crayon.Domain.Entities
{
    public class AccountService
    {
        public int AccountId { get; set; }
        public Account Account { get; set; }

        public int ServiceId { get; set; }
        public Service Service { get; set; }

        public string Status { get; set; }

        public int Quantity { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
