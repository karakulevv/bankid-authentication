using FluentValidation;
using Newtonsoft.Json;

namespace Application.Clients.Models.Requests;

public class BankIdCollectRequest : OrderReferenceDto
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