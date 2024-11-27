using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Shared
{
    public class Result
    {
        public bool IsSuccess { get; }
        public Error? Error { get; }

        public Result(bool status, Error? error)
        {
            IsSuccess = status;
            Error = error;
        }


        public static Result Success() => new(true, null);
        public static Result Failure(string message)
        {
            Error error = new(500, message);
            return new Result(false, error);
        }

        public static Result<T> Success<T>(T value) => new(value, true, null);

        public static Result<T> BadRequest<T>(T? value, string message)
        {
            Error error = new(400, message);

            return new Result<T>(value, false, error);
        }

        public static Result<T> BadRequest<T>(T? value, Error error)
        {
            return new Result<T>(value, false, error);
        }

        public static Result<T> Failure<T>(T? value, string message)
        {
            Error error = new(500, message);

            return new Result<T>(value, false, error);
        }
    }

    public class Result<T> : Result
    {
        public T? Value { get; set; }

        public Result(T? value, bool status, Error error) : base(status, error)
        {
            Value = value;
        }
    }
}
