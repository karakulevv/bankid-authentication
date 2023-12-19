using Application.Models.Responses;
using FluentValidation;
using MediatR;

namespace Application.Models.Requests;

public class QrCodeRequest : OrderReference, IRequest<QrCodeResponse>
{
}

public class QrCodeRequestValidator : AbstractValidator<QrCodeRequest>
{
    public QrCodeRequestValidator()
    {
        RuleFor(x => x).NotEmpty().NotNull();
        RuleFor(x => x.OrderRef).NotEmpty().NotNull();
    }
}