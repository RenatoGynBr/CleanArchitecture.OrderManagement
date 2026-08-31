using CleanArchitecture.OrderManagement.Domain.Enums;

namespace CleanArchitecture.OrderManagement.Application.DTOs
{
    public class CancelOrderRequest
    {
        public Guid OrderId { get; set; }
        public StatusType CurrentStatus { get; set; } // populate from DB or caller
    }
}
