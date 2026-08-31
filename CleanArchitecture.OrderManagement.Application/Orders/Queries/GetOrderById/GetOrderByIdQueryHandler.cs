using CleanArchitecture.OrderManagement.Application.Abstractions.Persistence;
using CleanArchitecture.OrderManagement.Application.DTOs;
using MediatR;

namespace CleanArchitecture.OrderManagement.Application.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler
    : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    private readonly IOrderRepository _repository;

    public GetOrderByIdQueryHandler(
        IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<OrderDto?> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (order is null)
            return null;

        var items = order.OrderItems
            .Select(x => new OrderItemDto(
                x.Id,
                x.ProductName,
                x.Quantity,
                x.UnitPrice,
                x.Quantity * x.UnitPrice))
            .ToList();

        return new OrderDto(
            order.Id,
            order.CustomerId,
            order.Status,
            order.CreatedAt,
            items,
            items.Sum(x => x.Total));
    }
}