using System;

namespace Bibliotech.Core.Abstractions;

public class Result
{
          public bool IsSuccess { get; }
          public bool IsFailure => !IsSuccess;
          public string Error { get; }
          public string[] Errors { get; }

          protected Result(bool isSuccess, string error, string[] errors)
          {
                    IsSuccess = isSuccess;
                    Error = error;
                    Errors = errors;
          }

          public static Result Success() => new(true, string.Empty, Array.Empty<string>());
          public static Result Failure(string error) => new(false, error, new[] { error });
          public static Result Failure(string[] errors) => new(false, string.Join(", ", errors), errors);

          public static implicit operator Result(string error) => Failure(error);
}

public class Result<T> : Result
{
          public T? Value {get;}

          protected Result(T? value, bool isSuccess, string error, string[] errors)
                    : base(isSuccess, error, errors)
                    {
                              Value = value
                    }

          public static Result<T> Success(T value) => new(value, true, string.Empty, Array.Empty<string>());
          public static new Result<T> Failure(string error) => new(default, false, error, new[] { error });
          public static new Result<T> Failure(string[] errors) => new(default, false, string.Join(", ", errors), errors);

          public static implicit operator Result<T>(T value) => Success(value);

          public static implicit operator Result<T>(string error) => Failure(error);
}
