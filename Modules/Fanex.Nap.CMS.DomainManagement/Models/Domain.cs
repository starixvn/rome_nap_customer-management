namespace Fanex.Nap.CMS.DomainManagement.Models
{
    using System;
    using Fanex.Nap.CMS.Infrastructure.Extensions;

    public class Domain
    {
        public Domain(
            int domainId,
            string domainName,
            string system,
            string site,
            string brandName,
            byte status,
            string restrictedCountries,
            string remark,
            DateTime? expiredDate,
            DateTime lastUpdated)
        {
            DomainId = domainId;
            DomainName = domainName;
            System = system;
            Site = site;
            BrandName = brandName;
            Status = status;
            RestrictedCountries = restrictedCountries;
            Remark = remark;
            ExpiredDate = expiredDate;
            LastUpdateTime = lastUpdated;
        }

        public int DomainId { get; }

        public string DomainName { get; }

        public string System { get; }

        public string Site { get; }

        public string SiteNames { get; set; }

        public string BrandName { get; }

        public byte Status { get; }

        public string StatusName { get; set; }

        public string RestrictedCountries { get; }

        public string RestrictedCountryNames { get; set; }

        public string Remark { get; }

        public DateTime? ExpiredDate { get; }

        public string ExpiredDateString => ExpiredDate.ToShortFormatedDateWithSlash();

        public DateTime LastUpdateTime { get; }

        public string LastUpdated => LastUpdateTime.ToLongDate24hWithSlash();
    }
}
