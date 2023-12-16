using Application.Models.Requests;
using Application.Models.Responses;
using MediatR;

namespace Application.Handlers;

public class BankIdStartHandler : IRequestHandler<BankIdStartRequest, SimplifiedBankIdStartResponse> 
{
    public async Task<SimplifiedBankIdStartResponse> Handle(BankIdStartRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}