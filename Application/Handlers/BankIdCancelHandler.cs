using Application.Models.Requests;
using Application.Models.Responses;
using MediatR;

namespace Application.Handlers;

public class BankIdCancelHandler : IRequestHandler<BankIdCancelRequest, BankIdCancelResponse>
{
    public async Task<BankIdCancelResponse> Handle(BankIdCancelRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}