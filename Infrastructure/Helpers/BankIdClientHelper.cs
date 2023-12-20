using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace Infrastructure.Helpers;

public static class BankIdClientHelper
{
    public static HttpRequestMessage CreateHttpRequestMessage<T>(string endpoint, T payload)
    {
        string serializedPayload = JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IgnoreNullValues = true
        });

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, endpoint);
        httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpRequest.Content = new StringContent(serializedPayload);
        httpRequest.Content!.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        return httpRequest;
    }

    public static X509Certificate2 GetCertificate(string clientCertificateThumbprint)
    {
        X509Certificate2 clientCertificate = FindCertificateInLocation(StoreLocation.CurrentUser, clientCertificateThumbprint);

        if (clientCertificate != null)
        {
            return clientCertificate;
        }

        clientCertificate = FindCertificateInLocation(StoreLocation.LocalMachine, clientCertificateThumbprint);

        if (clientCertificate != null)
        {
            return clientCertificate;
        }

        throw new Exception($"FATAL ERROR! No certificate was found with thumbprint: {clientCertificateThumbprint}");
    }

    private static X509Certificate2 FindCertificateInLocation(StoreLocation storeLocation, string thumbprint)
    {
        using var certStore = new X509Store(StoreName.My, storeLocation);
        certStore.Open(OpenFlags.ReadOnly);

        return FindCertificateByThumbprint(certStore, thumbprint);
    }

    private static X509Certificate2 FindCertificateByThumbprint(X509Store x509Store, string thumbprint) =>
        x509Store.Certificates.OfType<X509Certificate2>()
            .FirstOrDefault(c => c.Thumbprint?.ToUpper() == thumbprint?.ToUpper());
}