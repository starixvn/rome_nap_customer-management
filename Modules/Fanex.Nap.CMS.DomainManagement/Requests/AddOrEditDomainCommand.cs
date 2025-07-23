namespace Fanex.Nap.CMS.DomainManagement.Requests
{
    using System;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public class AddOrEditDomainCommand
    {
        public IdNameModel[] Domains { get; set; }

        public string System { get; set; }

        public string Site { get; set; }

        public byte Status { get; set; }

        public DateTime? ExpiredDate { get; set; }

        public string RestrictedCountries { get; set; }

        public string Remark { get; set; }

        public string BrandName { get; set; }

        public byte OfferType { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public bool IsEdit { get; set; }

        public virtual bool IsValid()
            => Domains != null && Domains.Length > 0
            && !string.IsNullOrWhiteSpace(System)
            && !string.IsNullOrWhiteSpace(Site)
            && Status >= 0;
    }
}
