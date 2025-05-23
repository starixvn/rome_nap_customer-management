namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class AssignSiteSettingCriteria : GetDuplicatedSiteSettingCriteria
    {
        public bool IsOverwrite { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public byte ActionID { get; set; }
        

        public override string GetSettingKey()
            => "NAP_CM_CustomerClassPolicySiteIns_Loop";

        
    }
}
