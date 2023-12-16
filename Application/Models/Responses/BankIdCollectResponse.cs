using Application.Models.Enums;
using System.Text.Json.Serialization;

namespace Application.Models.Responses;

public class BankIdCollectResponse
{
    public string OrderRef { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CollectStatus Status { get; set; }

    public string HintCode { get; set; }

    public CompletionData CompletionData { get; set; }
}