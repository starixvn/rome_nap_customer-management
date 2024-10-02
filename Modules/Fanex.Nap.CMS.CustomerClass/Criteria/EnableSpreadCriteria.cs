namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class EnableSpreadCriteria : CustomerClassCommandBaseCriteria
    {
        public int PolicyID { get; set; }
        public bool Enable { get; set; }
        public byte Type { get; set; }

        public override string GetSettingKey()
            => "NAP_CM_CustomerClassSpreadEnable";
    }
}
