using System.Runtime.Serialization;

namespace Application.Models.Enums;

public enum BankIdStatus
{
    [EnumMember(Value = "Ok")]
    Ok = 0,

    [EnumMember(Value = "Rfa1_OutstandingTransactionOrNoClient")]
    Rfa1_OutstandingTransactionOrNoClient = 1,

    [EnumMember(Value = "Rfa3_CancelledOrAlreadyInProgress")]
    Rfa3_CancelledOrAlreadyInProgress = 3,

    [EnumMember(Value = "Rfa4_AlreadyStarted")]
    Rfa4_AlreadyStarted = 4,

    [EnumMember(Value = "Rfa5_BankIdError")]
    Rfa5_BankIdError = 5,

    [EnumMember(Value = "Rfa6_UserCancel")]
    Rfa6_UserCancel = 6,

    [EnumMember(Value = "Rfa8_ExpiredTransaction")]
    Rfa8_ExpiredTransaction = 8,

    [EnumMember(Value = "Rfa9_UserSign")]
    Rfa9_UserSign = 9,

    [EnumMember(Value = "Rfa13_OutstandingTransaction")]
    Rfa13_OutstandingTransaction = 13,

    [EnumMember(Value = "Rfa14_StartedManual")]
    Rfa14_StartedManual = 14,

    [EnumMember(Value = "Rfa15_StartedAuto")]
    Rfa15_StartedAuto = 0xF,

    [EnumMember(Value = "Rfa16_CertificateError")]
    Rfa16_CertificateError = 0x10,

    [EnumMember(Value = "Rfa17_StartFailed")]
    Rfa17_StartFailed = 17,

    [EnumMember(Value = "Rfa21_IdentificationInProgress")]
    Rfa21_IdentificationInProgress = 21,

    [EnumMember(Value = "Rfa22_UnknownError")]
    Rfa22_UnknownError = 22
}