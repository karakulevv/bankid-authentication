using Application.Models.Responses;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;

namespace Application.Models.Requests;

public class CollectRequest : OrderReference, IRequest<CollectResponse>
{
    [JsonProperty("isAutoStart")]
    public bool IsAutoStart { get; set; }
}

public class CollectRequestValidator : AbstractValidator<CollectRequest>
{
    public CollectRequestValidator()
    {
        RuleFor(x => x).NotEmpty().NotNull();
        RuleFor(x => x.IsAutoStart).NotNull().WithState(x => x.IsAutoStart == true || x.IsAutoStart == false).WithMessage("IsAutoStart required");
        RuleFor(x => x.OrderRef).NotEmpty().NotNull();
    }
}