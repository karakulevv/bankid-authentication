using Application.Models.Responses;
using FluentValidation;
using MediatR;

namespace Application.Models.Requests;

public class BankIdQrCodeRequest : OrderReference, IRequest<BankIdQrCodeResponse>
{
}

public class BankIdQrCodeRequestValidator : AbstractValidator<BankIdQrCodeRequest>
{
    public BankIdQrCodeRequestValidator()
    {
        RuleFor(x => x).NotEmpty().NotNull();
        RuleFor(x => x.OrderRef).NotEmpty().NotNull();
    }
}