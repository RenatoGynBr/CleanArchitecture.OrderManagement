using MediatR;

namespace CleanArchitecture.OrderManagement.Application.Orders.Commands.ConfirmOrder;

public record ConfirmOrderCommand(Guid OrderId)
    : IRequest<bool>;