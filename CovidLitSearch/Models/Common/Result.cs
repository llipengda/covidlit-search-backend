﻿using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CovidLitSearch.Models.Common;

public class Result<T, E>
{
    public T? Data { get; set; }
    public E? Error { get; set; }
    public bool IsSuccess { get; }
    public bool IsError => !IsSuccess;

    public Result(T data)
    {
        Data = data;
        IsSuccess = true;
    }

    public Result(E error)
    {
        Error = error;
        IsSuccess = false;
    }

    public static implicit operator Result<T, E>(T data) => new(data);

    public static implicit operator Result<T, E>(E error) => new(error);

    public R Match<R>(Func<T, R> onSuccess, Func<E, R> onError) =>
        IsSuccess ? onSuccess(Data!) : onError(Error!);
}

public class Result<E>
{
    public E? Error { get; set; }
    public bool IsSuccess { get; }
    public bool IsError => !IsSuccess;

    public Result()
    {
        IsSuccess = true;
    }

    public Result(E error)
    {
        Error = error;
        IsSuccess = false;
    }

    public static implicit operator Result<E>(E error) => new(error);

    public R Match<R>(Func<R> onSuccess, Func<E, R> onError) =>
        IsSuccess ? onSuccess() : onError(Error!);
}
