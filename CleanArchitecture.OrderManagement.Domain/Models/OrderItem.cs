using CleanArchitecture.OrderManagement.Domain.Enums;

namespace CleanArchitecture.OrderManagement.Domain.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public required string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }



        public Order Order { get; set; } = null!; // Navigation Property - Each OrderItem belongs to one Order
    }
}
