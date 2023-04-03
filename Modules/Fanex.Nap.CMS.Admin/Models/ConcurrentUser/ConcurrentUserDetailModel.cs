namespace Fanex.Nap.CMS.Admin.Models.ConcurrentUser
{
    public class ConcurrentUserDetailModel : SMANumberModel
    {
        public string SiteLabel { get; set; }

        public string SiteName { get; set; }

        public int Total { get; set; }
    }
}
