using Application.Cache.Interfaces;
using Application.Clients.Interfaces;
using Application.Models.Requests;
using Application.Models.Responses;
using Application.Models.Dictionaries;
using MediatR;
using Serilog;
using Microsoft.Extensions.Options;
using Application.Clients.Options;
using Application.Models.Enums;
using Application.Exceptions;
using Application.Clients.Models;
using System.Security.Claims;
using Application.Authentication;
using Application.Clients.Models.Responses;

namespace Application.Handlers;

public class BankIdCollectHandler : IRequestHandler<CollectRequest, CollectResponse>
{
    private readonly IBankIdClient _bankIdClient;
    private readonly BankIdOptions _opt;
    private readonly ICache _cache;
    private readonly ILogger _logger;

    public BankIdCollectHandler(IBankIdClient bankIdClient, IOptions<BankIdOptions> opt, ICache cache, ILogger logger)
    {
        _bankIdClient = bankIdClient;
        _opt = opt.Value;
        _cache = cache;
        _logger = logger;
    }

    public async Task<CollectResponse> Handle(CollectRequest collectRequest, CancellationToken cancellationToken)
    {
        CollectResponse collectResponse;
        try
        {
            var startResponse = _cache.GetAsync<BankIdStartResponse>(collectRequest.OrderRef);
            if (startResponse == null)
            {
                _logger.Warning($"Failed to retrieve object from cache for key: {collectRequest.OrderRef}. Possible cache expiration.");
                throw new Exception($"Failed to retrieve object from cache for key: {collectRequest.OrderRef}. Possible cache expiration.");
            }

            collectResponse = await _bankIdClient.CollectAuthenticationAsync(collectRequest);
            if (RetryStatusesDictionary.RetryStatuses.ContainsKey(collectResponse.Status))
            {
                _logger.Warning($"Authmethod: BankID Failed to login user. Status: {collectResponse.Status}, Order Ref: {collectRequest.OrderRef}.");
                collectResponse = await RetryCollectAuthentication(collectRequest, collectResponse);
            }

            if (IsBankIdAuthenticationSuccessful(collectResponse))
            {
                var secret = Guid.NewGuid();
                await SaveClaimsToCacheAsync(new OrderReferenceDto { OrderRef = collectRequest.OrderRef}, collectResponse, secret);

                collectResponse.Secret = secret;

                return collectResponse;
            }

            if (ErrorMessageDictionary.ErrorMessages.ContainsKey(collectResponse.Status))
                throw new HttpResponseException($"Failed to authenticate BankID", 401, ErrorMessageDictionary.ErrorMessages[collectResponse.Status]);

            throw new HttpResponseException($"Failed to authenticate, Unknown error. Please try again!", 401,
                $"BankID failed to login user. Status: {collectResponse.Status}, Order Ref: {collectRequest.OrderRef}.");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unexpected error occurred during BankID collect");
            throw;
        }        
    }

    private async Task<CollectResponse> RetryCollectAuthentication(CollectRequest collectRequest, CollectResponse collectResponse, int retryCount = 0)
    {
        if (retryCount >= _opt.CollectRetryCount)
        {
            _logger.Warning($"After {retryCount} retries user failed to login to BankID. Status: {collectResponse.Status}, Order Ref: {collectRequest.OrderRef}.");
            return collectResponse;
        }

        await Task.Delay(2000);
        collectResponse = await _bankIdClient.CollectAuthenticationAsync(collectRequest);

        if (IsBankIdAuthenticationSuccessful(collectResponse))
        {
            _logger.Information("BankID successfully logged in user. Order Ref: {OrderRef}, Name: {Name}.", collectRequest.OrderRef, collectResponse.Name);
            return collectResponse;
        }

        if (RetryStatusesDictionary.RetryStatuses.ContainsKey(collectResponse.Status))
        {
            _logger.Warning($"Authmethod: BankID failed to login user. Status: {collectResponse.Status}, Order Ref: {collectRequest.OrderRef}.");
            collectResponse = await RetryCollectAuthentication(collectRequest, collectResponse, retryCount + 1);
        }

        return collectResponse;
    }

    private bool IsBankIdAuthenticationSuccessful(CollectResponse result)
    {
        return result.IsCompleted && result.Status == BankIdStatus.Ok;
    }

    private async Task SaveClaimsToCacheAsync(OrderReferenceDto request, CollectResponse response, Guid secret)
    {
        var claims = new ClaimsObject(
                        new Claim[]
                        {
                                new Claim(IdentityModel.JwtClaimTypes.Subject, response.Ssn),
                                new Claim(IdentityModel.JwtClaimTypes.Name, request.OrderRef),
                                new Claim(ClaimTypes.GivenName, response.GivenName),
                                new Claim(ClaimTypes.Surname, response.Surname),
                                new Claim(nameof(request.OrderRef), request.OrderRef),
                                new Claim(nameof(response.Ssn), response.Ssn),
                        }
                    );

        await _cache.Set(secret.ToString(), claims, TimeSpan.FromMinutes(1));
    }
}