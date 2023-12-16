using Application.Models.Requests;
using Application.Models.Responses;
using MediatR;

namespace Application.Handlers;

public class BankIdCollectHandler : IRequestHandler<BankIdCollectRequest, BankIdCollectResponse>
{
    public async Task<BankIdCollectResponse> Handle(BankIdCollectRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}