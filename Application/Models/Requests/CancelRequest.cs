using FluentValidation;
using MediatR;

namespace Application.Models.Requests;

public class CancelRequest : OrderReference, IRequest<Unit>
{
}

public class CancelRequestValidator : AbstractValidator<CancelRequest>
{
    public CancelRequestValidator()
    {
        RuleFor(x => x).NotEmpty().NotNull();
        RuleFor(x => x.OrderRef).NotEmpty().NotNull();
    }
}