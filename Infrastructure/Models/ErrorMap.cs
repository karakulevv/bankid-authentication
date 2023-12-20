using Application.Models.Enums;
using System.Net;

namespace Infrastructure.Models;

public class ErrorMap
{
    public HttpStatusCode HttpStatusCode { get; }

    public string ErrorCode { get; }

    public BankIdStatus ResultStatus { get; }

    public ErrorMap(HttpStatusCode httpStatusCode, string errorCode, BankIdStatus resultStatus)
    {
        HttpStatusCode = httpStatusCode;
        ErrorCode = errorCode;
        ResultStatus = resultStatus;
    }
}