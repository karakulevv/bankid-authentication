using Application.Models.Responses;
using FluentValidation;
using MediatR;

namespace Application.Models.Requests;

public class CancelRequest : OrderReference, IRequest<CancelResponse>
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