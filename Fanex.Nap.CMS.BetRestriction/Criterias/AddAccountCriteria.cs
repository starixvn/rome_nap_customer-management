namespace Fanex.Nap.CMS.BetRestriction.Criterias
{
    using Fanex.Data.Repository;

    public class AddAccountCriteria : AddEditAccountCriteria
    {
        public string UserName { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_BetRestriction_CustomerIns";
        }
    }
}
