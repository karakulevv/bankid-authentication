using Application.Models.Enums;
using System.Text.Json.Serialization;

namespace Application.Models.Responses;

public class BankIdCollectUserResponse
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public BankIdStatus Status { get; }

    public bool IsCompleted { get; }

    public string? Ssn { get; }

    public string? Name { get; set; }

    public string? GivenName { get; set; }

    public string? Surname { get; set; }

    public BankIdCollectUserResponse(User userData)
    {
        Status = BankIdStatus.Ok;
        IsCompleted = true;
        Ssn = userData.PersonalNumber;
        Name = userData.Name;
        GivenName = userData.GivenName;
        Surname = userData.Surname;
    }

    public BankIdCollectUserResponse(BankIdStatus status, bool isCompleted)
    {
        Status = status;
        IsCompleted = isCompleted;
    }
}
