using Fanex.Nap.CMS.CustomerClass.Requests;

namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class EditSpreadRuleCriteria : AddSpreadRuleCriteria
    {
        internal EditSpreadRuleCriteria(AddEditSpreadRuleRequest request, int userId, string ip) : base(request, userId, ip)
        {
        }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassSpreadUpd";
    }
}
