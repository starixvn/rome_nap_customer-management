namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class AddMultiSpreadRuleCriteria : CustomerClassCommandBaseCriteria
    {
        public string DataSpread { get; set; }

        public override string GetSettingKey()
           => "NAP_CM_CustomerClassSpread_MultipleInsert";
    }
}
