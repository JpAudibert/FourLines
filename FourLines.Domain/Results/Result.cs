namespace FourLines.Domain.Results;

public class Result
{
    public bool IsSuccess { get; private set; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; set; }
    public object Value { get; set; } = default!;

    private Result(bool isSuccess, object? value, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Operation not allowed", nameof(error));
        }

        IsSuccess = isSuccess;
        Value = value ?? new { };
        Error = error;
    }

    public static Result Success(object value) => new(true, value, Error.None);
    public static Result Failure(Error error) => new(false, null, error);

}
