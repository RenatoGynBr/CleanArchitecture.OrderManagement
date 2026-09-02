namespace CleanArchitecture.OrderManagement.Application.Common;

public class Result
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error? Error { get; }

    protected Result(
        bool isSuccess,
        Error? error = null)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success()
        => new(true);

    public static Result Failure(Error error)
        => new(false, error);
}

public record Error(string Code, string Message);


public class Result<T> : Result
{
    public T? Value { get; }

    private Result(
        bool isSuccess,
        T? value,
        Error? error = null)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public static Result<T> Success(T value)
        => new(
            true,
            value);

    public static new Result<T> Failure(Error error)
        => new(
            false,
            default,
            error);
}