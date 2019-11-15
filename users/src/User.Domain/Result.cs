using System;

namespace User.Domain
{
    public class Result
    {
        public virtual bool IsSuccess { get; }
        public string Error { get; }
        public string Description { get; }
        public virtual object Value { get; }

        public Result()
        {
            IsSuccess = true;
        }

        public Result(object value)
        {
            IsSuccess = true;
            Value = value;
        }

        public Result(string error, string description)
        {
            IsSuccess = false;
            Error = error ?? throw new ArgumentNullException(nameof(error));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        private static readonly OkResult _ok = new OkResult();
        public static OkResult Ok()
            => _ok;

        public static ErrorResult Fail(string error, string description)
            => new ErrorResult(error, description);
    }

    public class OkResult : Result
    {
        public override bool IsSuccess => true;
    }

    public class OkResult<T> : Result
    {
        public override bool IsSuccess => true;
        public new T Value { get; }

        public OkResult(T value)
        {
            Value = value;
        }
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
