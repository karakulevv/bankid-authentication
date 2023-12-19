using Application.Models.Requests;
using Application.Clients.Models.Requests;
using Application.Clients.Models.Responses;
using Application.Clients.Interfaces;

namespace Infrastructure.Clients;

public class BankIdClient : IBankIdClient
{
    public Task CancelAuthenticationAsync(BankIdCancelRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<BankIdCollectResponse> CollectAuthenticationAsync(CollectRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<BankIdStartResponse> StartAuthenticationAsync(BankIdStartRequest request)
    {
        throw new NotImplementedException();
    }
}