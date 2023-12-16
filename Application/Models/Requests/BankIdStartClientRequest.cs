using Newtonsoft.Json;

namespace Application.Models.Requests;

public class BankIdStartClientRequest
{
    [JsonProperty("endUserIp")]
    public string EndUserIp { get; set; }
}