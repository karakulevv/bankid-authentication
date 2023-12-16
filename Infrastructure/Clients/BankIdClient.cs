using Application.Models.Requests;
using Application.Models.Responses;
using Infrastructure.Clients.Interfaces;

namespace Infrastructure.Clients;

public class BankIdClient : IBankIdClient
{
    public Task CancelAuthenticationAsync(BankIdCancelRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<BankIdCollectUserResponse> CollectAuthenticationAsync(BankIdCollectRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<BankIdStartResponse> InitiateAuthenticationAsync(BankIdStartClientRequest request)
    {
        throw new NotImplementedException();
    }
}