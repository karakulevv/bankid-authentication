using Application.Models.Requests;
using Application.Models.Responses;
using MediatR;

namespace Application.Handlers;

public class BankIdQrCodeHandler : IRequestHandler<BankIdQrCodeRequest, BankIdQrCodeResponse>
{
    public Task<BankIdQrCodeResponse> Handle(BankIdQrCodeRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}