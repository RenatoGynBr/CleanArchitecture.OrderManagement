using CleanArchitecture.OrderManagement.Application.Abstractions.Persistence;
using CleanArchitecture.OrderManagement.Application.Common;
using CleanArchitecture.OrderManagement.Domain.Models;
using MediatR;

namespace CleanArchitecture.OrderManagement.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, Result<Guid>>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<Guid>> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Items is null || request.Items.Count == 0)
        {
            return Result<Guid>.Failure(OrderErrors.InvalidStatus);
        }

        var order = new Order(request.CustomerId);

        foreach (var item in request.Items)
        {
            try
            {
                order.AddItem(
                    item.ProductName,
                    item.Quantity,
                    item.UnitPrice);
            }
            catch (ArgumentException ex)
            {
                return Result<Guid>.Failure(
                    new Error(
                        "Order.InvalidItem",
                        ex.Message));
            }
        }

        await _orderRepository.AddAsync(
            order,
            cancellationToken);

        await _orderRepository.SaveChangesAsync(
            cancellationToken);

        return Result<Guid>.Success(order.Id);
    }
}