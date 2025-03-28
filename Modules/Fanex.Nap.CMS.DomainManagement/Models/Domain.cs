namespace Fanex.Nap.CMS.DomainManagement.Models
{
    using System;
    using Fanex.Nap.CMS.Infrastructure.Extensions;

    public class Domain
    {
        public int DomainId { get; set; }

        public string DomainName { get; set; }

        public string System { get; set; }

        public string Site { get; set; }

        public string SiteNames { get; set; }

        public string BrandName { get; set; }

        public byte Status { get; set; }

        public string StatusName { get; set; }

        public string RestrictedCountries { get; set; }

        public string RestrictedCountryNames { get; set; }

        public string Remark { get; set; }

        public DateTime? ExpiredDate { get; set; } = default;

        public string ExpiredDateString => ExpiredDate.ToShortFormatedDateWithSlash();

        public DateTime LastUpdateTime { get; set; }

        public string LastUpdated => LastUpdateTime.ToLongDate24hWithSlash();
    }
}
