namespace Fanex.Nap.CMS.CustomerClass.Models
{
    using System.Collections.Generic;

    internal class PagedSiteSetting
    {
        public PagedSiteSetting(int totalItems, IEnumerable<SiteSetting> siteSettings)
        {
            TotalItems = totalItems;
            SiteSettings = siteSettings;
        }

        public int TotalItems { get; }

        public IEnumerable<SiteSetting> SiteSettings { get; }
    }
}
