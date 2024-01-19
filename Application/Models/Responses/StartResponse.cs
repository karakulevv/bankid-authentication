namespace Application.Models.Responses;

public class StartResponse
{
    public string OrderRef { get; set; }

    public string BankIdAutoStartUrl { get; set; }

    public string IosBankAutoStartUrl { get; set; }

    public StartResponse(string orderRef, string autoStartToken, string returnUrl)
    {
        OrderRef = orderRef;
        BankIdAutoStartUrl = $"bankid:///?autostarttoken={autoStartToken}&redirect=null";
        IosBankAutoStartUrl = $"https://app.bankid.com/?autostarttoken={autoStartToken}&redirect=null";
    }
}