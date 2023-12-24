using Application.Models.Enums;

namespace Application.Models.Dictionaries;

public static class ErrorMessageDictionary
{
    public static readonly Dictionary<BankIdStatus, string> ErrorMessages = new Dictionary<BankIdStatus, string>()
    {
        {BankIdStatus.Rfa3_CancelledOrAlreadyInProgress, "Action cancelled. Please try again." },
        {BankIdStatus.Rfa6_UserCancel, "Action cancelled" },
        {BankIdStatus.Rfa4_AlreadyStarted, "An identification or signing for this personal number is already started. Please try again" },
        {BankIdStatus.Rfa8_ExpiredTransaction, "The BankID app is not responding. Please check that the program is started and that you have internet access. If you don’t have a valid BankID you can get one from your bank. Try again." },
        {BankIdStatus.Rfa5_BankIdError, "Internal error. Please try again." },
        {BankIdStatus.Rfa22_UnknownError, "Unknown error. Please try again." },
        {BankIdStatus.Rfa1_OutstandingTransactionOrNoClient, "Start your BankID app." },
        {BankIdStatus.Rfa17_StartFailed, "The BankID app couldn’t be found on your computer or mobile device. Please install it and order a BankID from your internet bank. Install the app from your app store or https://install.bankid.com." },
        {BankIdStatus.Rfa16_CertificateError, "The BankID you are trying to use is revoked or too old. Please use another BankID or order a new one from your internet bank." },
        {BankIdStatus.Rfa13_OutstandingTransaction, "Trying to start your BankID app." },
        {BankIdStatus.Rfa9_UserSign, "Enter your security code in the BankID app and select Identify or Sign."}
    };
}