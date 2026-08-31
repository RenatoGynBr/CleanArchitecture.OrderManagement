using CleanArchitecture.OrderManagement.Application.Abstractions.Persistence;
using CleanArchitecture.OrderManagement.Domain.Enums;
using MediatR;
using System.Net.NetworkInformation;

namespace CleanArchitecture.OrderManagement.Application.Orders.Commands.ConfirmOrder;

public class ConfirmOrderCommandHandler
    : IRequestHandler<ConfirmOrderCommand, bool>
{
    private readonly IOrderRepository _repository;

    public ConfirmOrderCommandHandler(
        IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(
    ConfirmOrderCommand request,
    CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(
            request.OrderId,
            cancellationToken);

        if (order is null)
            return false;

        order.Confirm();

        await _repository.SaveChangesAsync(
            cancellationToken);

        return true;
    }

    public void Confirm()
    {
        if (Status != StatusType.Pending)
        {
            throw new InvalidOperationException(
                "Only pending orders can be confirmed.");
        }

        Status = StatusType.Confirmed;
    }
    public StatusType Status { get; set; }
}