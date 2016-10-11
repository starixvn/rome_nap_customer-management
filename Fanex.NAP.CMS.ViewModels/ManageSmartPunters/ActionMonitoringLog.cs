using System;

namespace Fanex.NAP.CMS.ViewModels.ManageSmartPunters
{
    public class ActionMonitoringLog
    {
        public DateTime LogTime { get; set; }

        public string Admin { get; set; }

        public string Username { get; set; }

        public string Level { get; set; }

        public string ActionName { get; set; }

        public string FunctionName { get; set; }

        public string IP { get; set; }

        public bool IsRestricted { get; set; }
    }
}
