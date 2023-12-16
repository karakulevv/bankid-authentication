namespace Application.Exceptions;

public class BankIdException : Exception
{
    public BankIdException()
    {
    }

    public BankIdException(string message)
        : base(message)
    {
    }

    public BankIdException(string message, Exception inner)
        : base(message, inner)
    {
    }
}