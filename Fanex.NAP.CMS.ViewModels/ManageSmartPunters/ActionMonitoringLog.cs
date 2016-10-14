using System;
using Fanex.NAP.CMS.Core.Web;

namespace Fanex.NAP.CMS.ViewModels.ManageSmartPunters
{
    public class ActionMonitoringLog
    {
        private string _username;

        public DateTime LogDate { get; set; }

        public string Admin { get; set; }

        public string Username
        {
            get { return Upline == null ? _username : string.Format("{0} ({1})", _username, Upline); }
            set { _username = value; }
        }

        public string RoleName
        {
            get
            {
                return CacheRef.GetLevel((byte?)RoleId);
            }
        }

        public string Upline { get; set; }

        public int RoleId { get; set; }

        public string ActionName { get; set; }

        public string FunctionName { get; set; }

        public string IP { get; set; }

        public bool IsRestricted { get; set; }
    }
}
