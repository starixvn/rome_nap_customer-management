namespace Fanex.Nap.CMS.CustomerClass.Requests
{
    public class SearchCustomerClassSettingRequest
    {
        public string Site { get; set; }

        public short CustomerClass { get; set; }

        public string SportTypes { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
