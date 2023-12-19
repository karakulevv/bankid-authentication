using Application.Models.Enums;
using System.Text.Json.Serialization;

namespace Application.Models.Responses;

public class CollectResponse
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public BankIdStatus Status { get; }

    public bool IsCompleted { get; }

    public string? Ssn { get; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? GivenName { get; set; }

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