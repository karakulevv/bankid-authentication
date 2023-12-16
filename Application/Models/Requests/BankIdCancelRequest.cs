using Application.Models.Responses;
using FluentValidation;
using MediatR;

namespace Application.Models.Requests;

public class BankIdCancelRequest : OrderReference, IRequest<BankIdCancelResponse>
{
}

public class BankIdCancelRequestValidator : AbstractValidator<BankIdCancelRequest>
{
    public BankIdCancelRequestValidator()
    {
        RuleFor(x => x).NotEmpty().NotNull();
        RuleFor(x => x.OrderRef).NotEmpty().NotNull();
    }
}