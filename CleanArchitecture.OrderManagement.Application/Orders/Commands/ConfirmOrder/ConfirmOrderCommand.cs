using CleanArchitecture.OrderManagement.Application.Common;
using MediatR;

public record ConfirmOrderCommand(Guid OrderId)
    : IRequest<Result>;