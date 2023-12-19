using Newtonsoft.Json;

namespace Application.Clients.Models.Requests;

public class BankIdStartRequest
{
    [JsonProperty("endUserIp")]
    public string EndUserIp { get; set; }
}