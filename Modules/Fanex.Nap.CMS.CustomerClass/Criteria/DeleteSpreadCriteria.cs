namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class DeleteSpreadCriteria : CustomerClassCommandBaseCriteria
    {
        public int PolicyID { get; set; }
        public byte Type { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassSpreadDel";
    }
}
