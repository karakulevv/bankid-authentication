namespace Application.Exceptions;

public class HttpObjectResult
{
    public int Status { get; set; }

    public string Message { get; set; }

    public string Details { get; set; }
}

public class HttpResponseException : Exception
{
    public HttpResponseException(string message, int status, string details = null)
    {
        Value = new HttpObjectResult
        {
            Details = details,
            Message = message,
            Status = status
        };
    }

    public HttpObjectResult Value { get; }
}