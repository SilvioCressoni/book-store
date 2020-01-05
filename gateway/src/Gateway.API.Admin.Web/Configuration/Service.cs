namespace Gateway.API.Admin.Web.Configuration
{
    public class Service
    {
        public string Name { get; set; }
        public bool IsSecure { get; set; }
        public string Address { get; set; }
        public Policy Policy { get; set; }
    }
}
