namespace FourLines.Domain.Results;

public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; set; }
    public T Value { get; set; } = default!;

    private Result(bool isSuccess, T? value, Error error)
    {
        if (isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Operation not allowed", nameof(error));
        }

        IsSuccess = isSuccess;
        Value = value!;
        Error = error;
    }

    public static Result<T> Success(T value) => new(true, value, Error.None);
    public static Result<T> Failure(Error error) => new(false, default, error);

}
