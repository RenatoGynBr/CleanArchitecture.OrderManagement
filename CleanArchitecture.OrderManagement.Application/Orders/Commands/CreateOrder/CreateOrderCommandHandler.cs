using CleanArchitecture.OrderManagement.Application.Abstractions.Persistence;
using CleanArchitecture.OrderManagement.Domain.Enums;
using CleanArchitecture.OrderManagement.Domain.Models;
using MediatR;

namespace CleanArchitecture.OrderManagement.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Guid> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var orderId = Guid.NewGuid();

        var order = new Order(request.CustomerId);

        foreach (var item in request.Items)
        {
            order.AddItem(
                item.ProductName,
                item.Quantity,
                item.UnitPrice);
        }

        await _orderRepository.AddAsync(
            order,
            cancellationToken);

        await _orderRepository.SaveChangesAsync(
            cancellationToken);

        return order.Id;
    }
}