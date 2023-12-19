using Application.Models.Enums;
using Newtonsoft.Json;

namespace Application.Clients.Models.Responses;

public class BankIdStartResponse
{
    [JsonProperty("status")]
    public BankIdStatus Status { get; set; }

    [JsonProperty("orderRef")]
    public string OrderRef { get; set; }

    [JsonProperty("autoStartToken")]
    public string AutoStartToken { get; set; }

    [JsonProperty("qrStartToken")]
    public string QrStartToken { get; set; }

    [JsonProperty("qrStartSecret")]
    public string QrStartSecret { get; set; }

    [JsonProperty("authStartTime")]
    public DateTime AuthStartTime { get; set; }

    public BankIdStartResponse() { }

    public BankIdStartResponse(BankIdStartResponse bankIdStart)
    {
        Status = BankIdStatus.Ok;
        OrderRef = bankIdStart.OrderRef;
        AutoStartToken = bankIdStart.AutoStartToken;
        QrStartToken = bankIdStart.QrStartToken;
        QrStartSecret = bankIdStart.QrStartSecret;
        AuthStartTime = DateTime.UtcNow;
    }

    public BankIdStartResponse(BankIdStatus status)
    {
        Status = status;
    }
}