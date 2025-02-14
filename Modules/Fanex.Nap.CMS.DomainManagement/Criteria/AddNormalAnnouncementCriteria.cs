namespace Fanex.Nap.CMS.DomainManagement.Criteria
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Configurations;

    public class AddNormalAnnouncementCriteria : CriteriaBase
    {
        public AddNormalAnnouncementCriteria(string domainList)
            => DomainList = domainList;

        public string DomainList {get;}

        public bool IsTest { get; } = AppSettings.IsTest;

        public override string GetSettingKey()
            => "NAP_Domain_SetMessage";

        public override bool IsValid()
            => true;
    }
}
