using Application.Clients.Models.Requests;
using Application.Clients.Models.Responses;
using Application.Models.Requests;
using Application.Models.Responses;

namespace Application.Clients.Interfaces;

public interface IBankIdClient
{
    Task<BankIdStartResponse> StartAuthenticationAsync(BankIdStartRequest request);
    Task<CollectResponse> CollectAuthenticationAsync(CollectRequest request);
    Task CancelAuthenticationAsync(CancelRequest request);
}