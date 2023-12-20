using Application.Models.Requests;
using Application.Clients.Models.Requests;
using Application.Clients.Models.Responses;
using Application.Clients.Interfaces;
using System.Net;
using Infrastructure.Models;
using Application.Exceptions;
using System.Text.Json.Serialization;
using System.Text.Json;
using Application.Clients.Options;
using Microsoft.Extensions.Options;
using Infrastructure.Helpers;
using Application.Models.Enums;
using Application.Clients.Models;
using Application.Models.Responses;

namespace Infrastructure.Clients;

public class BankIdClient : IBankIdClient
{
    private readonly IHttpClientFactory _clientFactory;
    public const string ClientName = "BankIdClient";
    private readonly BankIdOptions _opt;

    private static readonly WaitMap[] WaitResultMappings = new WaitMap[11]
{
            new WaitMap("outstandingTransaction", true, BankIdStatus.Rfa13_OutstandingTransaction),
            new WaitMap("outstandingTransaction", false, BankIdStatus.Rfa1_OutstandingTransactionOrNoClient),
            new WaitMap("noClient", null, BankIdStatus.Rfa1_OutstandingTransactionOrNoClient),
            new WaitMap("started", false, BankIdStatus.Rfa14_StartedManual),
            new WaitMap("started", true, BankIdStatus.Rfa15_StartedAuto),
            new WaitMap("userSign", null, BankIdStatus.Rfa9_UserSign),
            new WaitMap("expiredTransaction", null, BankIdStatus.Rfa8_ExpiredTransaction),
            new WaitMap("userCancel", null, BankIdStatus.Rfa6_UserCancel),
            new WaitMap("cancelled", null, BankIdStatus.Rfa3_CancelledOrAlreadyInProgress),
            new WaitMap("startFailed", null, BankIdStatus.Rfa17_StartFailed),
            new WaitMap("certificateErr", null, BankIdStatus.Rfa16_CertificateError)
};

    private static readonly ErrorMap[] ErrorResultMappings = new ErrorMap[4]
    {
            new ErrorMap(HttpStatusCode.BadRequest, "alreadyInProgress", BankIdStatus.Rfa4_AlreadyStarted),
            new ErrorMap(HttpStatusCode.RequestTimeout, "requestTimeout", BankIdStatus.Rfa5_BankIdError),
            new ErrorMap(HttpStatusCode.InternalServerError, "internalError", BankIdStatus.Rfa5_BankIdError),
            new ErrorMap(HttpStatusCode.ServiceUnavailable, "Maintenance", BankIdStatus.Rfa5_BankIdError)
    };

    public BankIdClient(IHttpClientFactory clientFactory, IOptions<BankIdOptions> opt)
    {
        _clientFactory = clientFactory;
        _opt = opt.Value;
    }

    public async Task<BankIdStartResponse> StartAuthenticationAsync(BankIdStartRequest request)
    {
        try
        {
            var clientResponse = await PostAsync<BankIdStartResponse>(request, _opt.Endpoints.BankIdInitiate);
            if (clientResponse.ErrorResponse == null)
            {
                return new BankIdStartResponse(clientResponse.OkResponse!);
            }

            ErrorResponse errorResponse = clientResponse.ErrorResponse;
            if (errorResponse.ErrorCode == "invalidParameters")
            {
                throw new BankIdException("Unexpected BankID error. Code: " + errorResponse.ErrorCode + ". Details: " + errorResponse.Details);
            }

            return new BankIdStartResponse(BankIdStatus.Rfa22_UnknownError);
        }
        catch (Exception ex)
        {
            throw new Exception($"Client failed to initiate BankID authentication.", ex);

        }
    }

    public async Task<CollectResponse> CollectAuthenticationAsync(CollectRequest request)
    {
        try
        {
            var clientResponse = await PostAsync<BankIdCollectResponse>(new OrderReferenceDto { OrderRef = request.OrderRef }, _opt.Endpoints.BankIdCollect);

            if (clientResponse.ErrorResponse == null)
            {
                BankIdCollectResponse okResponse = clientResponse.OkResponse!;
                if (okResponse.Status == CollectStatus.Complete)
                {
                    var user = okResponse.CompletionData.User;
                    return new CollectResponse(user.PersonalNumber,user.Name, user.Surname, user.GivenName);
                }

                CollectResponse bankIdWaitAuthenticationResult = MapWaitResultByHintCode(okResponse.Status, okResponse.HintCode, request.IsAutoStart);
                if (bankIdWaitAuthenticationResult != null)
                {
                    return bankIdWaitAuthenticationResult;
                }

                return new CollectResponse((okResponse.Status == CollectStatus.Failed) ? BankIdStatus.Rfa22_UnknownError : BankIdStatus.Rfa21_IdentificationInProgress, okResponse.Status == CollectStatus.Failed);
            }

            ErrorResponse errorResponse = clientResponse.ErrorResponse;
            CollectResponse bankIdWaitAuthenticationResult2 = MapErrorResultByStatusAndErrorCode(clientResponse.HttpStatusCode, errorResponse.ErrorCode);
            if (bankIdWaitAuthenticationResult2 != null)
            {
                return bankIdWaitAuthenticationResult2;
            }

            if (clientResponse.HttpStatusCode == HttpStatusCode.BadRequest && errorResponse.ErrorCode != "invalidParameters")
            {
                return new CollectResponse(BankIdStatus.Rfa22_UnknownError, isCompleted: true);
            }

            throw new BankIdException($"Unexpected BankID result. Order:{request.OrderRef}. Response:{JsonSerializer.Serialize(clientResponse)}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Client failed to collect BankID authentication.", ex);
        }
    }

    public Task CancelAuthenticationAsync(BankIdCancelRequest request)
    {
        throw new NotImplementedException();
    }

    private async Task<ClientResponse<T>> PostAsync<T>(object request, string urlLastPart) where T : class
    {
        var (httpStatusCode, content, errorResponse) = await PostAsync(request, urlLastPart);
        if (httpStatusCode == HttpStatusCode.OK)
        {
            T okResponse = Deserialize<T>(content);
            return new ClientResponse<T>(httpStatusCode, okResponse);
        }

        return new ClientResponse<T>(httpStatusCode, errorResponse);
    }

    private async Task<(HttpStatusCode StatusCode, string? Content, ErrorResponse? ErrorResponse)> PostAsync(object request, string urlLastPart)
    {
        var httpClient = _clientFactory.CreateClient(ClientName);
        var httpRequest = BankIdClientHelper.CreateHttpRequestMessage(urlLastPart, request);

        HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest);
        string text = await httpResponse.Content.ReadAsStringAsync();
        if (httpResponse.IsSuccessStatusCode)
        {
            return (httpResponse.StatusCode, text, null);
        }

        if (string.IsNullOrEmpty(text))
        {
            throw new BankIdException($"Unexpected BankID response. HTTP Status:{httpResponse.StatusCode}. Content is empty.");
        }

        ErrorResponse errorResponse;
        var deserializedResponse = TryDeserialize<ErrorResponse>(text, out errorResponse);

        if (!deserializedResponse || errorResponse == null || errorResponse.ErrorCode == null)
        {
            throw new BankIdException($"Unexpected BankID response. HTTP Status:{httpResponse.StatusCode}. Content:{text}.");
        }

        return (httpResponse.StatusCode, text, errorResponse);
    }

    private CollectResponse? MapWaitResultByHintCode(CollectStatus collectStatus, string hintCode, bool isAutoStart)
    {
        string hintCode2 = hintCode;
        WaitMap waitMap = WaitResultMappings.SingleOrDefault((WaitMap a) => a.HintCode == hintCode2 && (a.IsAutoStart == isAutoStart || !a.IsAutoStart.HasValue));
        if (waitMap == null)
        {
            return null;
        }

        return new CollectResponse(waitMap.ResultStatus, collectStatus != CollectStatus.Pending);
    }

    private CollectResponse? MapErrorResultByStatusAndErrorCode(HttpStatusCode httpStatusCode, string errorCode)
    {
        ErrorMap errorMap = ErrorResultMappings.SingleOrDefault((ErrorMap a) => a.HttpStatusCode == httpStatusCode && a.ErrorCode == errorCode);
        if (errorMap == null)
        {
            return null;
        }

        return new CollectResponse(errorMap.ResultStatus, isCompleted: true);
    }

    private static T Deserialize<T>(string content)
    {
        return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    private static bool TryDeserialize<T>(string content, out T response) where T : class
    {
        try
        {
            response = Deserialize<T>(content);
            return true;
        }
        catch (Exception)
        {
            response = null;
            return false;
        }
    }

    private string SerializeForLog<T>(T obj)
    {
        JsonStringEnumConverter item = new JsonStringEnumConverter();
        JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
        jsonSerializerOptions.Converters.Add(item);
        return JsonSerializer.Serialize(obj, jsonSerializerOptions);
    }
}