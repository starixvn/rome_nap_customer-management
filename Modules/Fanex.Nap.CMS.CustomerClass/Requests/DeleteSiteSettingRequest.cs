namespace Fanex.Nap.CMS.CustomerClass.Requests
{
    using System.Collections.Generic;
    
    public class DeleteSiteSettingRequest
    {
        public IEnumerable<DeleteSiteSettingModel> SiteSettings { get; set; }
    }

    public class DeleteSiteSettingModel
    {
        public int PolicyId { get; set; }
        public string Site { get; set; }
        public string Currency { get; set; }
        public string Platform { get; set; }
    }
}
