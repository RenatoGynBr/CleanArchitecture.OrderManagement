using CleanArchitecture.OrderManagement.Application.Abstractions.Persistence;
using CleanArchitecture.OrderManagement.Application.Common;
using CleanArchitecture.OrderManagement.Domain.Enums;
using MediatR;

public class ConfirmOrderCommandHandler
    : IRequestHandler<ConfirmOrderCommand, Result>
{
    private readonly IOrderRepository _repository;

    public ConfirmOrderCommandHandler(
        IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(
        ConfirmOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _repository.GetByIdAsync(
            request.OrderId,
            cancellationToken);

        if (order is null)
            return Result.Failure(
                OrderErrors.NotFound(request.OrderId));

        if (order.Status != StatusType.Pending)
            return Result.Failure(
                OrderErrors.InvalidStatus);

        order.Confirm();

        await _repository.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }
}