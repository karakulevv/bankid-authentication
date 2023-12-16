namespace Application.Models.Responses;

public class SimplifiedBankIdStartResponse
{
    public string OrderRef { get; set; }

    public string BankIdAutoStartUrl { get; set; }

    public string IosBankAutoStartUrl { get; set; }

    public SimplifiedBankIdStartResponse(BankIdStartResponse startResponse, string returnUrl)
    {
        OrderRef = startResponse.OrderRef;
        BankIdAutoStartUrl = $"bankid:///?autostarttoken={startResponse.AutoStartToken}&redirect=null";
        IosBankAutoStartUrl = $"https://app.bankid.com/?autostarttoken={startResponse.AutoStartToken}&redirect={(string.IsNullOrEmpty(returnUrl) ? "null" : returnUrl)}";
    }
}