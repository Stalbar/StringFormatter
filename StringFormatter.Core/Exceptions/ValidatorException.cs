namespace StringFormatter.Core.Exceptions;

public class ValidatorException: Exception
{
    public ValidatorException(string message)
        : base(message)
    {

    }
}