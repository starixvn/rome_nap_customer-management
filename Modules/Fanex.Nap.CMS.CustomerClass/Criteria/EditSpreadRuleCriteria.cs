using Fanex.Nap.CMS.CustomerClass.Requests;

namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class EditSpreadRuleCriteria : AddSpreadRuleCriteria
    {
        public override string GetSettingKey()
            => "NAP_CM_CustomerClassSpreadUpd";
    }
}
