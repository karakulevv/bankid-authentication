using Application.Clients.Interfaces;
using Application.Models.Requests;
using Application.Models.Responses;
using MediatR;

namespace Application.Handlers;

public class BankIdQrCodeHandler : IRequestHandler<QrCodeRequest, QrCodeResponse>
{
    private readonly IBankIdClient _bankIdClient;

    public BankIdQrCodeHandler(IBankIdClient bankIdClient)
    {
        _bankIdClient = bankIdClient;
    }

    public Task<QrCodeResponse> Handle(QrCodeRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}