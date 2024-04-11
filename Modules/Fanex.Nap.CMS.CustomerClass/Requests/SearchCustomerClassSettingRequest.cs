namespace Fanex.Nap.CMS.CustomerClass.Requests
{
    public class SearchCustomerClassSettingRequest
    {
        public string Site { get; set; }

        public string CustomerClass { get; set; }

        public string CustomerClassDetail { get; set; }

        public string SportTypes { get; set; }

        public string Currencies { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
