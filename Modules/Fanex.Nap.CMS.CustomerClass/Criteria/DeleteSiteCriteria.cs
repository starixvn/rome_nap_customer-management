namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class DeleteSiteCriteria : CustomerClassCommandBaseCriteria
    {
        public string PolicySites { get; set; }

        public override string GetSettingKey()
            => "NAP_CM_CustomerClassPolicySiteDel";
    }
    
}
