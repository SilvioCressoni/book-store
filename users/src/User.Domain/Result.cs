using System;

namespace User.Domain
{
    public class Result
    {
        public virtual bool IsSuccess { get; }
        public string Error { get; }
        public string Description { get; }

        public Result()
        {
            IsSuccess = true;
        }

        public Result(string error, string description)
        {
            IsSuccess = false;
            Error = error ?? throw new ArgumentNullException(nameof(error));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public static OkResult Ok { get; } = new OkResult();

        public static ErrorResult Fail(string error, string description)
            => new ErrorResult(error, description);
    }

    public class OkResult : Result
    {
        public override bool IsSuccess => true;

    }

    public class ErrorResult : Result
    {
        public override bool IsSuccess => false;

        public ErrorResult(string error, string description) 
            : base(error, description)
        {
        }
    }
}
