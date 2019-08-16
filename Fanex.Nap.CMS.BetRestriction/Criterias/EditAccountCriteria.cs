namespace Fanex.Nap.CMS.BetRestriction.Criterias
{
    using Fanex.Data.Repository;

    public class EditAccountCriteria : AddEditAccountCriteria
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_BetRestriction_CustomerUpd";
        }
    }
}
