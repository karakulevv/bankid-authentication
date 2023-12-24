using Application.Cache.Interfaces;
using Application.Clients.Interfaces;
using Application.Models.Requests;
using MediatR;

namespace Application.Handlers;

public class BankIdCancelHandler : IRequestHandler<CancelRequest, Unit>
{
    private readonly IBankIdClient _bankIdClient;
    private readonly ICache _cache;

    public BankIdCancelHandler(IBankIdClient bankIdClient, ICache cache)
    {
        _bankIdClient = bankIdClient;
        _cache = cache;
    }

    public async Task<Unit> Handle(CancelRequest request, CancellationToken cancellationToken)
    {
        await _bankIdClient.CancelAuthenticationAsync(request);
        await _cache.Remove(request.OrderRef);

        return Unit.Value;
    }
}