using Application.Models.Responses;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;

namespace Application.Models.Requests;

public class StartRequest : IRequest<StartResponse>
{
    [JsonProperty("returnUrl")]
    public string ReturnUrl { get; set; }
}

public class StartRequestValidator : AbstractValidator<StartRequest>
{
    public StartRequestValidator()
    {
        RuleFor(x => x).NotEmpty().NotNull();
        RuleFor(x => x.ReturnUrl).NotEmpty().NotNull().WithMessage("returnUrl parameter missing");
    }
}