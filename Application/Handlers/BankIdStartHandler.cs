using Application.Cache.Interfaces;
using Application.Clients.Interfaces;
using Application.Clients.Models.Requests;
using Application.Clients.Options;
using Application.Exceptions;
using Application.Models.Dictionaries;
using Application.Models.Requests;
using Application.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;

namespace Application.Handlers;

public class BankIdStartHandler : IRequestHandler<StartRequest, StartResponse>
{
    private readonly IBankIdClient _bankIdClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly BankIdOptions _opt;
    private readonly ILogger _logger;
    private readonly ICache _cache;

    public BankIdStartHandler(IBankIdClient bankIdClient, IOptions<BankIdOptions> opt, IHttpContextAccessor httpContextAccessor, ICache cache, ILogger logger)
    {
        _bankIdClient = bankIdClient;
        _opt = opt.Value;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _cache = cache;
    }

    public async Task<StartResponse> Handle(StartRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _bankIdClient.StartAuthenticationAsync(new BankIdStartRequest { EndUserIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString() });

            if (ErrorMessageDictionary.ErrorMessages.ContainsKey(response.Status))
            {
                _logger.Warning($"Failed to start BankID authentication. Status: {response.Status}, Order Ref: {response.OrderRef}.");
                throw new HttpResponseException($"Failed to start BankID authentication.", 401, ErrorMessageDictionary.ErrorMessages[response.Status]);
            }

            await _cache.Set(response.OrderRef, response, TimeSpan.FromMinutes(_opt.CacheBankIdOrderMin));

            return new StartResponse(response.OrderRef, response.AutoStartToken, request.ReturnUrl);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unexpected error occurred during BankID start");
            throw;
        }
    }
}