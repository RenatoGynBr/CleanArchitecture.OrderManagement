using CleanArchitecture.OrderManagement.Domain.Enums;
using CleanArchitecture.OrderManagement.Domain.Models;

public class Order
{
    public Guid Id { get; private set; }

    public Guid CustomerId { get; private set; }

    public StatusType Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public List<OrderItem> OrderItems { get; private set; } = new();

    public Order()
    {
    }

    public Order(Guid customerId)
    {
        Id = Guid.NewGuid();

        CustomerId = customerId;

        Status = StatusType.Pending;

        CreatedAt = DateTime.UtcNow;
    }

    public void Confirm()
    {
        if (Status != StatusType.Pending)
            throw new InvalidOperationException(
                "Only pending orders can be confirmed.");

        Status = StatusType.Confirmed;
    }

    public void Cancel()
    {
        if (Status == StatusType.Cancelled)
            return;

        Status = StatusType.Cancelled;
    }

    public void AddItem(
        string productName,
        int quantity,
        decimal unitPrice)
    {
        if (quantity <= 0)
            throw new ArgumentException(
                "Quantity must be greater than zero.");

        if (unitPrice < 0)
            throw new ArgumentException(
                "Unit price cannot be negative.");

        OrderItems.Add(new OrderItem
        {
            Id = Guid.NewGuid(),
            OrderId = Id,
            ProductName = productName,
            Quantity = quantity,
            UnitPrice = unitPrice
        });
    }
}