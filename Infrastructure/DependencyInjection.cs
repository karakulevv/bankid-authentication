using Application.Clients.Interfaces;
using Application.Clients.Options;
using Infrastructure.Clients;
using Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var bankidOptions = configuration.GetSection(nameof(BankIdOptions));
        services.Configure<BankIdOptions>(bankidOptions);
        var bankIdOptions = bankidOptions.Get<BankIdOptions>();

        services.AddHttpClient<IBankIdClient, BankIdClient>(client =>
        {
            client.BaseAddress = new Uri(bankIdOptions!.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(30.0);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }).ConfigurePrimaryHttpMessageHandler((IServiceProvider services) =>
        {
            var httpClientHandler = new HttpClientHandler();
            // Retrieve and add the client certificate
            string fullPath = Path.Combine(Environment.CurrentDirectory, bankIdOptions!.CertificatePath, bankIdOptions!.CertificateName);
            var certificate = BankIdClientHelper.LoadCertificate(fullPath, bankIdOptions!.CertificatePassword);

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