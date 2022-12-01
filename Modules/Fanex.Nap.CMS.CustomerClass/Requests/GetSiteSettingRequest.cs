namespace Fanex.Nap.CMS.CustomerClass.Requests
{
    public class GetSiteSettingRequest
    {
        public string Sites { get; set; }

        public int PolicyId { get; set; }

        public string Platforms { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
