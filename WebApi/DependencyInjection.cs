using Application.Clients.Interfaces;
using Application.Clients.Options;
using Infrastructure.Clients;
using Infrastructure.Helpers;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        var bankIdOptions = configuration.GetSection(nameof(BankIdOptions)).Get<BankIdOptions>();
        services.AddHttpClient<IBankIdClient, BankIdClient>(client =>
        {
            client.BaseAddress = new Uri(bankIdOptions!.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(30.0);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }).ConfigurePrimaryHttpMessageHandler((IServiceProvider services) =>
        {
            var httpClientHandler = new HttpClientHandler();
            // Retrieve and add the client certificate
            var certificate = BankIdClientHelper.GetCertificate(bankIdOptions.ClientCertificateThumbprint);

            // Configure certificate validation callback and certificate based on options
            if (bankIdOptions.IgnoreServerCertificateErrors)
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (HttpRequestMessage message, X509Certificate2 certificate2, X509Chain chain, SslPolicyErrors errors) => true;
            }

            httpClientHandler.ClientCertificates.Add(certificate);
            return httpClientHandler;
        });

        return services;
    }
}
