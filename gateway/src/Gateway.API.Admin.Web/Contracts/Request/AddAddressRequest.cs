namespace Gateway.API.Admin.Web.Contracts.Request
{
    public class AddAddressRequest
    {
        public string Line { get; set; }
        public int Number { get; set; }
        public string PostCode { get; set; }
    }
}
