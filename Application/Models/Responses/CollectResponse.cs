using Application.Models.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using JsonConverter = Newtonsoft.Json.JsonConverter;
using JsonConverterAttribute = Newtonsoft.Json.JsonConverterAttribute;

namespace Application.Models.Responses;

public class CollectResponse
{
    [JsonProperty("status")]
    [JsonConverter(typeof(StringEnumConverter))]
    public BankIdStatus Status { get; }

    [JsonProperty("isCompleted")]
    public bool IsCompleted { get; }

    [JsonProperty("ssn")]
    public string? Ssn { get; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("givenname")]
    public string? GivenName { get; set; }

    [JsonProperty("surname")]
    public string? Surname { get; set; }

    [JsonProperty("secret")]
    public Guid Secret { get; set; }

    public CollectResponse(string ssn, string name, string surname, string givenName)
    {
        Status = BankIdStatus.Ok;
        IsCompleted = true;
        Ssn = ssn;
        Name = name;
        GivenName = givenName;
        Surname = surname;
    }

    public CollectResponse(BankIdStatus status, bool isCompleted)
    {
        Status = status;
        IsCompleted = isCompleted;
    }
}