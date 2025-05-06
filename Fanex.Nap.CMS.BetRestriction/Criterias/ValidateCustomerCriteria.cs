namespace Fanex.Nap.CMS.BetRestriction.Criterias
{
    using Infrastructure.Criteria;

    public class ValidateCustomerCriteria : CommandBaseCriteria
    {
        public string UserName { get; set; }

        public short SportType { get; set; }

        public int CustID { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_BetRestriction_ValidateCustomer";
        }

        public override bool IsValid()
            => true;
    }
}
