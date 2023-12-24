using Application.Models.Enums;

namespace Application.Models.Dictionaries;

public static class RetryStatusesDictionary
{
    public static readonly Dictionary<BankIdStatus, string> RetryStatuses = new Dictionary<BankIdStatus, string>()
    {
        {BankIdStatus.Rfa1_OutstandingTransactionOrNoClient, "Start your BankID app." },
        {BankIdStatus.Rfa13_OutstandingTransaction, "Trying to start your BankID app." },
        {BankIdStatus.Rfa9_UserSign, "Enter your security code in the BankID app and select Identify or Sign."},
        {BankIdStatus.Rfa21_IdentificationInProgress, "Identification or signing in progress." }
    };
}