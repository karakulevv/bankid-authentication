namespace Application.Clients.Models;

public class CompletionData
{
    public User User { get; set; }

    public Device Device { get; set; }

    public Cert Cert { get; set; }

    public string Signature { get; set; }

    public string OcspResponse { get; set; }
}

public class User
{
    public string PersonalNumber { get; set; }

    public string Name { get; set; }

    public string GivenName { get; set; }

    public string Surname { get; set; }
}

public class Device
{
    public string IpAddress { get; set; }
}

public class Cert
{
    public string NotBefore { get; set; }

    public string NotAfter { get; set; }
}