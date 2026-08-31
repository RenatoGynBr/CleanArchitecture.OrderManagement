using CleanArchitecture.OrderManagement.Application.DTOs;
using MediatR;

namespace CleanArchitecture.OrderManagement.Application.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(Guid Id)
    : IRequest<OrderDto?>;