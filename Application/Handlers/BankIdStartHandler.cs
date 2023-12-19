using Application.Clients.Interfaces;
using Application.Models.Requests;
using Application.Models.Responses;
using MediatR;

namespace Application.Handlers;

public class BankIdStartHandler : IRequestHandler<StartRequest, StartResponse> 
{
    private readonly IBankIdClient _bankIdClient;

    public BankIdStartHandler(IBankIdClient bankIdClient)
    {
        _bankIdClient = bankIdClient;
    }

    public async Task<StartResponse> Handle(StartRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}