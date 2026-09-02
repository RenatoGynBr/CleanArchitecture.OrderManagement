using CleanArchitecture.OrderManagement.Application.Abstractions.Persistence;
using CleanArchitecture.OrderManagement.Application.Common;
using CleanArchitecture.OrderManagement.Application.DTOs;
using MediatR;

namespace CleanArchitecture.OrderManagement.Application.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler
    : IRequestHandler<GetOrderByIdQuery, Result<OrderDto>>
{
    private readonly IOrderRepository _repository;

    public GetOrderByIdQueryHandler(
        IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<OrderDto>> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (order is null)
        {
            return Result<OrderDto>.Failure(OrderErrors.NotFound(request.Id));
        }

        var items = order.OrderItems
            .Select(item => new OrderItemDto(
                item.Id,
                item.ProductName,
                item.Quantity,
                item.UnitPrice,
                item.Quantity * item.UnitPrice))
            .ToList();

        var orderDto = new OrderDto(
            order.Id,
            order.CustomerId,
            order.Status,
            order.CreatedAt,
            items,
            items.Sum(item => item.Total));

        return Result<OrderDto>.Success(orderDto);
    }
}