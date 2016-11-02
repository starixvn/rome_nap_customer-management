using System;
using Fanex.Data.Repository;

namespace Fanex.NAP.CMS.Criteria.ManageSmartPunters
{
    public class GetActionMonitoringLogsCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string UserName { get; set; }

        public string Admin { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_UserLog_SmartPunterSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
