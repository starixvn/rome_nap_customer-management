namespace Fanex.Nap.CMS.DomainManagement.Criteria
{
    using Fanex.Data.Repository;

    public class GetDomainByIdCriteria : CriteriaBase
    {
        public GetDomainByIdCriteria(string domainId)
            => DomainId = domainId;

        public string DomainId { get; }

        public override string GetSettingKey()
            => "NAP_Domain_SelByID";

        public override bool IsValid()
            => true;
    }
}
