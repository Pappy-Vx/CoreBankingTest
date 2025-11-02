using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBankingTest.Application.Common.Models
{
    public record Result
    {
        public bool IsSuccess { get; init; }
        public string[] Errors { get; init; } = Array.Empty<string>();
        public string Message { get; init; } = string.Empty;

        public static Result Success()=> new() { IsSuccess = true  };
        public static Result Failure(params string[] errors) => new() { IsSuccess = false, Errors = errors  };
    }

    public record Result<T>:Result
    {
        public T? Data { get; init; }
        public new static Result<T> Success(T data) => new() { IsSuccess = true, Data = data };
        public new static Result<T> Failure(params string[] errors) => new() { IsSuccess = false, Errors = errors };
    }
}
