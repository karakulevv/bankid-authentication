namespace Application.Clients.Options;

public class BankIdOptions
{
    public string BaseUrl { get; set; } 

    public string CertificateName { get; set; }

    public string CertificatePassword { get; set; }

    public string CertificatePath { get; set; }

    public bool IgnoreServerCertificateErrors { get; set; }

    public string ClientCertificateThumbprint { get; set; }

    public int CacheBankIdOrderMin { get; set; }

    public int CollectRetryCount { get; set; }

    public BankIdEndpoints Endpoints { get; set; }
}

public class BankIdEndpoints
{
    public string BankIdInitiate { get; set; }
    public string BankIdCollect { get; set; }
    public string BankIdCancel { get; set; }
}