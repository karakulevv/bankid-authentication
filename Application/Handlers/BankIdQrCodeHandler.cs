using Application.Cache.Interfaces;
using Application.Clients.Models.Responses;
using Application.Models.Requests;
using Application.Models.Responses;
using MediatR;
using Serilog;
using System.Security.Cryptography;
using System.Text;

namespace Application.Handlers;

public class BankIdQrCodeHandler : IRequestHandler<QrCodeRequest, QrCodeResponse>
{
    private readonly ICache _cache;
    private readonly ILogger _logger;
    private static string qrPrefix = "bankid";

    public BankIdQrCodeHandler(ICache cache, ILogger logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<QrCodeResponse> Handle(QrCodeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var bankIdStartResponse = await _cache.GetAsync<BankIdStartResponse>(request.OrderRef);

            if (bankIdStartResponse == null)
            {
                _logger.Warning($"Failed to retrieve object from cache for key: {request.OrderRef}. Possible cache expiration.");
                throw new Exception($"Failed to retrieve object from cache for key: {request.OrderRef}. Possible cache expiration.");
            }

            DateTime timeNow = DateTime.UtcNow;
            var timeSeconds = (int)(timeNow - bankIdStartResponse.AuthStartTime).TotalSeconds;
            var qrTime = timeSeconds.ToString();

            var qrAuthCode = HashHMAC(bankIdStartResponse.QrStartSecret, qrTime);
            return new QrCodeResponse { QrData = $"{qrPrefix}.{bankIdStartResponse.QrStartToken}.{qrTime}.{qrAuthCode}" };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unexpected error occurred during QR Code generation");
            throw;
        }
    }

    public string HashHMAC(string key, string message)
    {
        var keyBytes = Encoding.UTF8.GetBytes(key);
        var messageBytes = Encoding.UTF8.GetBytes(message);

        using (var hash = new HMACSHA256(keyBytes))
        {
            var hashBytes = hash.ComputeHash(messageBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                // x2 means hexadecimal format
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}