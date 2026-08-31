using CleanArchitecture.OrderManagement.Application.Abstractions.Persistence;
using CleanArchitecture.OrderManagement.Application.DTOs;
using MediatR;

namespace CleanArchitecture.OrderManagement.Application.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler
    : IRequestHandler<GetOrdersQuery, IReadOnlyList<OrderDto>>
{
    private readonly IOrderRepository _repository;

    public GetOrdersQueryHandler(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<OrderDto>> Handle(
        GetOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var orders =
            await _repository.GetAllAsync(cancellationToken);

        return orders.Select(order =>
        {
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
        }).ToList();
    }
}