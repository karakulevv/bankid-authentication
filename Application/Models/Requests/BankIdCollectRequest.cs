using Application.Models.Responses;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;

namespace Application.Models.Requests;

public class BankIdCollectRequest : OrderReference, IRequest<BankIdCollectResponse>
{
    [JsonProperty("isAutoStart")]
    public bool IsAutoStart { get; set; }
}

public class BankIdCollectRequestValidator : AbstractValidator<BankIdCollectRequest>
{
    public BankIdCollectRequestValidator()
    {
        RuleFor(x => x).NotEmpty().NotNull();
        RuleFor(x => x.IsAutoStart).NotNull().WithState(x => x.IsAutoStart == true || x.IsAutoStart == false).WithMessage("IsAutoStart required");
        RuleFor(x => x.OrderRef).NotEmpty().NotNull();
    }
}