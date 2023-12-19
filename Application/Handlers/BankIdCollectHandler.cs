using Application.Clients.Interfaces;
using Application.Models.Requests;
using Application.Models.Responses;
using MediatR;

namespace Application.Handlers;

public class BankIdCollectHandler : IRequestHandler<CollectRequest, CollectResponse>
{
    private readonly IBankIdClient _bankIdClient;

    public BankIdCollectHandler(IBankIdClient bankIdClient)
    {
        _bankIdClient = bankIdClient;
    }

    public async Task<CollectResponse> Handle(CollectRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}