using Application.Orders.Commands.Create;
using Application.Orders.Commands.TakeOrder;
using FluentValidation;

namespace Application.Orders.Commands.Create;

public class TakeOrderCommandValidator : AbstractValidator<TakeOrderCommand>
{
    public TakeOrderCommandValidator()
    {
        RuleFor(r => r.OrderId)
             .GreaterThanOrEqualTo(1)
             .WithMessage("La orden no es válida.");
    }
}