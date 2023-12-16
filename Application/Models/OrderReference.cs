using Newtonsoft.Json;

namespace Application.Models;

public class OrderReference
{
    [JsonProperty("orderRef")]
    public string OrderRef { get; set; }
}