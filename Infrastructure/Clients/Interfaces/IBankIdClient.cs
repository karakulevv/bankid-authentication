using Application.Models.Requests;
using Application.Models.Responses;
using Infrastructure.Models.Requests;

namespace Infrastructure.Clients.Interfaces;

public interface IBankIdClient
{
    Task<BankIdStartResponse> InitiateAuthenticationAsync(BankIdStartClientRequest request);
    Task<BankIdCollectUserResponse> CollectAuthenticationAsync(BankIdCollectRequest request);
    Task CancelAuthenticationAsync(BankIdCancelRequest request);
}