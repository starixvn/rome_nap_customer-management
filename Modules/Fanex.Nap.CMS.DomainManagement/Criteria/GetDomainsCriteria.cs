namespace Fanex.Nap.CMS.DomainManagement.Criteria
{
    using Fanex.Data.Repository;

    public class GetDomainsCriteria : CriteriaBase
    {
        public GetDomainsCriteria(string system, string site, string status, string remark, string countries)
        {
            System = system ?? string.Empty;
            Site = site ?? string.Empty;
            Status = status ?? string.Empty;
            Remark = remark;
            Countries = countries;
        }

        public string System { get; }

        public string Site { get; }

        public string Status { get; }

        public string Remark { get; }

        public string Countries { get; }

        public override string GetSettingKey()
            => "NAP_Domain_Sel";
    }
}
