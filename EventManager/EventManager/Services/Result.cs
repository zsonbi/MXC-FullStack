using Microsoft.AspNetCore.Mvc;

namespace EventManager.Services
{
    public class Result<T> : Result
    {
        public T? Data { get; set; }
        public Result(T? data, bool success, string message, int statusCode) : base(success, message, statusCode)
        {
            Data = data;
        }

        public Result()
        {

        }

        public static Result<T> Ok(T data) => new(data, true, "Success", 200);
        public static Result<T> Created(T data) => new(data, true, "Created", 201);
        public static new Result<T> Fail(string message) => new(default, false, message, 400);
        public static new Result<T> FailOnCreate(string message) => new(default, false, message, 409);
        public static new Result<T> FailNotFound(string message) => new(default, false, message, 404);
        
        public override IActionResult ReturnAsActionResult()
        {
          return  new ObjectResult(this.Data is null ? this.Message : this.Data) { StatusCode = StatusCode };
        }
    }

    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; } = default;

        protected Result(bool success, string message, int statusCode)
        {
            Success = success;
            Message = message;
            StatusCode = statusCode;
        }
        public Result()
        {

        }

        public virtual IActionResult ReturnAsActionResult()
        {
            return new ObjectResult(this.Message) { StatusCode = StatusCode };
        }

        public static Result<T> Ok<T>(T data) => Result<T>.Ok(data);
        public static Result Ok(string message = "success") => new(true, message, 200);
        public static Result Fail(string message) => new(false, message, 400);
        public static Result FailOnCreate(string message) => new(false, message, 409);
        public static Result FailNotFound(string message) => new(false, message, 404);
    }

}
