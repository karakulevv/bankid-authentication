using Application.Models.Responses;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;

namespace Application.Models.Requests;

public class BankIdStartRequest : IRequest<SimplifiedBankIdStartResponse>
{
    [JsonProperty("returnUrl")]
    public string ReturnUrl { get; set; }
}

public class BankIdStartRequestValidator : AbstractValidator<BankIdStartRequest>
{
    public BankIdStartRequestValidator()
    {
        RuleFor(x => x).NotEmpty().NotNull();
        RuleFor(x => x.ReturnUrl).NotEmpty().NotNull().WithMessage("returnUrl parameter missing");
    }
}