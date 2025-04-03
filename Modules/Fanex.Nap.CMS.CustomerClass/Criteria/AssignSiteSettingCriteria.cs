namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests;

    internal class AssignSiteSettingCriteria : GetDuplicatedSiteSettingCriteria
    {
        public AssignSiteSettingCriteria(string policySites, bool isOverwrite)
            : base(policySites)
        {
            IsOverwrite = isOverwrite;
        }

        public AssignSiteSettingCriteria(
            string policySites,
            bool isOverwride,
            int userId,
            string ip) : base(policySites)
        {
            PolicySites = policySites;
            IsOverwrite = isOverwride;
            UserId = userId;
            IP = ip;
        }

        public bool IsOverwrite { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public byte ActionID { get; set; }
        

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassPolicySiteIns_Loop";

        
    }
}
