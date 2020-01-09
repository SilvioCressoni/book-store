namespace Gateway.API.Admin.Web.Contracts.Response
{
    public class Result<T>
    {
        public T Value { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorCode { get; set; }
        public string Description { get; set; }
    }

    public class Result
    {
        public bool IsSuccess { get; set; } = false;
        public string ErrorCode { get; set; }
        public string Description { get; set; }
    }
}
