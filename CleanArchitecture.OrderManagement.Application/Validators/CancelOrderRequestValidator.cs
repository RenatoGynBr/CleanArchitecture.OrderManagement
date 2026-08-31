using CleanArchitecture.OrderManagement.Application.DTOs;
using CleanArchitecture.OrderManagement.Domain.Enums;
using FluentValidation;

namespace CleanArchitecture.OrderManagement.Application.Validators
{
    public class CancelOrderRequestValidator : AbstractValidator<CancelOrderRequest>
    {
        public CancelOrderRequestValidator()
        {
            RuleFor(x => x.CurrentStatus)
                .Equal(StatusType.Pending)
                .WithMessage("Only orders with status 'Pending' can be cancelled.");
        }
    }
}
