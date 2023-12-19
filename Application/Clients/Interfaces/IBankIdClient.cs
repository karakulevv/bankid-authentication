using Application.Clients.Models.Requests;
using Application.Clients.Models.Responses;
using Application.Models.Requests;

namespace Application.Clients.Interfaces;

public interface IBankIdClient
{
    Task<BankIdStartResponse> StartAuthenticationAsync(BankIdStartRequest request);
    Task<BankIdCollectResponse> CollectAuthenticationAsync(CollectRequest request);
    Task CancelAuthenticationAsync(BankIdCancelRequest request);
}