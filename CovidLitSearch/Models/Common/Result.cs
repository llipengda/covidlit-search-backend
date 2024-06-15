namespace CovidLitSearch.Models.Common;

public class Result<T, E>
{
    private readonly T? data;

    private readonly E? error;

    public bool IsSuccess { get; }

    public bool IsError => !IsSuccess;

    public Result(T data)
    {
        this.data = data;
        IsSuccess = true;
    }

    public Result(E error)
    {
        this.error = error;
        IsSuccess = false;
    }

    public static implicit operator Result<T, E>(T data) => new(data);

    public static implicit operator Result<T, E>(E error) => new(error);

    public R Match<R>(Func<T, R> onSuccess, Func<E, R> onError) =>
        IsSuccess ? onSuccess(data!) : onError(error!);

    public T Unwrap() => IsSuccess ? data! : throw new Exception(error!.ToString());
}

public class Result<E>
{
    private readonly E? error;

    public bool IsSuccess { get; }

    public bool IsError => !IsSuccess;

    public Result()
    {
        IsSuccess = true;
    }

    public Result(E error)
    {
        this.error = error;
        IsSuccess = false;
    }

    public static implicit operator Result<E>(E error) => new(error);

    public R Match<R>(Func<R> onSuccess, Func<E, R> onError) =>
        IsSuccess ? onSuccess() : onError(error!);

    public void Unwrap() => _ = IsSuccess ? true : throw new Exception(error!.ToString());
}
