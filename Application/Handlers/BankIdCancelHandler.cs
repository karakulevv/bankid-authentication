using Application.Clients.Interfaces;
using Application.Models.Requests;
using Application.Models.Responses;
using MediatR;

namespace Application.Handlers;

public class BankIdCancelHandler : IRequestHandler<CancelRequest, CancelResponse>
{
    private readonly IBankIdClient _bankIdClient;

    public BankIdCancelHandler(IBankIdClient bankIdClient)
    {
        _bankIdClient = bankIdClient;
    }

    public async Task<CancelResponse> Handle(CancelRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}