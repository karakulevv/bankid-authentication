using Application.Models.Enums;

namespace Infrastructure.Models;

public class WaitMap
{
    public string HintCode { get; }

    public bool? IsAutoStart { get; }

    public BankIdStatus ResultStatus { get; }

    public WaitMap(string hintCode, bool? isAutoStart, BankIdStatus resultStatus)
    {
        HintCode = hintCode;
        IsAutoStart = isAutoStart;
        ResultStatus = resultStatus;
    }
}