namespace Fanex.Nap.CMS.DomainManagement.Criteria
{
    using Fanex.Data;
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class AddOrEditDomainCriteria : CommandBaseCriteria
    {
        public AddOrEditDomainCriteria(string domainList, int userId, string ip, bool isEdit)
        {
            DomainList = domainList;
            UserId = userId;
            IP = ip;
            IsEdit = isEdit;
        }

        public string DomainList { get; }

        public int UserId { get; }

        public string IP { get; }

        public string DomainListOutput { get; set; } = string.Empty;

        [SpParam(Ignored = true)]
        public bool IsEdit { get; }

        public override string GetSettingKey()
            => IsEdit ? "NAP_Domain_Upd" : "NAP_Domain_Ins";

        public override bool IsValid()
            => true;
    }
}
