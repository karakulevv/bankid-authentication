using FluentValidation;

namespace Application.Clients.Models.Requests;

public class BankIdCancelRequest : OrderReferenceDto
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