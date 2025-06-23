namespace Fanex.Nap.CMS.DomainManagement.Criteria
{
    using Fanex.Data;
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class AddOrEditDomainCriteria : CommandBaseCriteria
    {
        public string DomainList { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public string DomainListOutput { get; set; } = string.Empty;

        [SpParam(Ignored = true)]
        public bool IsEdit { get; set; }

        public override string GetSettingKey()
            => IsEdit ? "NAP_Domain_Upd" : "NAP_Domain_Ins";

        public override bool IsValid()
            => true;
    }
}
