namespace Fanex.Nap.CMS.DomainManagement.Criteria
{
    using Fanex.Data.Repository;

    public class RemoveDomainCriteria : CriteriaBase
    {
        public RemoveDomainCriteria(int domainId, int userId, string ip)
        {
            DomainId = domainId;
            UserId = userId;
            IP = ip;
        }

        public int DomainId { get; }

        public int UserId { get; }

        public string IP { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "NAP_Domain_Del";
    }
}
