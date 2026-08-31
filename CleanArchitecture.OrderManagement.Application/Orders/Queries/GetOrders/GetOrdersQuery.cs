using CleanArchitecture.OrderManagement.Application.DTOs;
using MediatR;

namespace CleanArchitecture.OrderManagement.Application.Orders.Queries.GetOrders;

public record GetOrdersQuery()
    : IRequest<IReadOnlyList<OrderDto>>;