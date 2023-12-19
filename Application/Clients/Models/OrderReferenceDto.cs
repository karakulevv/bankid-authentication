using Newtonsoft.Json;

namespace Application.Clients.Models;

public class OrderReferenceDto
{
    [JsonProperty("orderRef")]
    public string OrderRef { get; set; }
}