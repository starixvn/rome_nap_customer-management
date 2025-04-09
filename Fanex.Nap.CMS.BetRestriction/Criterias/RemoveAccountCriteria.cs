namespace Fanex.Nap.CMS.BetRestriction.Criterias
{
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class RemoveAccountCriteria : CommandBaseCriteria
    {
        public int CustId { get; set; }

        public int UserId { get; set; }

        public short? SportType { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_BetRestriction_CustomerDel";
        }

        public override bool IsValid()
            => true;
    }
}
