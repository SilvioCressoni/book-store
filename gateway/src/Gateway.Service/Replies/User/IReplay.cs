namespace Users.Web.Proto
{
    public interface IReplay
    {
        bool IsSuccess { get; }
        string ErrorCode { get; }
        string Description { get; }
        
        object Value { get; }

        string ToStringLog()
            => $"[{nameof(IsSuccess)}: {IsSuccess}]" +
               $"[{nameof(ErrorCode)}: {ErrorCode}]" +
               $"[{nameof(Description)}: {Description}]";
    }

    public interface IReply<T> : IReplay
    {
        new T Value { get; }

        object IReplay.Value => Value;
    }
    
}
