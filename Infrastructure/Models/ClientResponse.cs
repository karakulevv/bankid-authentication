using System.Net;

namespace Infrastructure.Models;

public class ClientResponse<T> where T : class
{
    public T? OkResponse { get; }

    public ErrorResponse? ErrorResponse { get; }

    public HttpStatusCode HttpStatusCode { get; }

    public ClientResponse(HttpStatusCode httpStatusCode, T okResponse)
    {
        OkResponse = okResponse;
        HttpStatusCode = httpStatusCode;
    }

    public ClientResponse(HttpStatusCode httpStatusCode, ErrorResponse errorResponse)
    {
        ErrorResponse = errorResponse;
        HttpStatusCode = httpStatusCode;
    }
}