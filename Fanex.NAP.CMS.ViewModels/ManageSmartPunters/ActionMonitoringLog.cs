using System;
using Fanex.NAP.CMS.Core.Web;

namespace Fanex.NAP.CMS.ViewModels.ManageSmartPunters
{
    public class ActionMonitoringLog
    {
        public DateTime LogDate { get; set; }

        public string Admin { get; set; }

        public string Username { get; set; }

        public string RoleName
        {
            get
            {
                return CacheRef.GetLevel((byte?)RoleId);
            }
        }

        public int RoleId { get; set; }

        public string ActionName { get; set; }

        public string FunctionName { get; set; }

        public string IP { get; set; }

        public bool IsRestricted { get; set; }
    }
}
