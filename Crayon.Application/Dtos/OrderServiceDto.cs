namespace Crayon.Application.Dtos
{
    public sealed class OrderServiceDto
    {
        public int ServiceId { get; set; }

        public int Quantity {  get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
