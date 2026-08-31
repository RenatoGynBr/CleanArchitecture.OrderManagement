using MediatR;

namespace CleanArchitecture.OrderManagement.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(
    Guid CustomerId,
    IReadOnlyCollection<CreateOrderItemRequest> Items)
    : IRequest<Guid>;