namespace Fanex.Nap.CMS.DomainManagement.Criteria
{
    using Fanex.Data.Repository;

    public class GetDomainsCriteria : CriteriaBase
    {
        public string System { get; set; } = string.Empty;

        public string Site { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string OfferType { get; set; } = string.Empty;

        public string Remark { get; set; }

        public string Countries { get; set; }

        public override string GetSettingKey()
            => "NAP_Domain_Sel";

        public override bool IsValid()
            => true;
    }
}
