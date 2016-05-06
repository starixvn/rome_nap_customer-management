namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VrAutoAdjustmentReport
{
    using System;

    public class VrAutoAdjustmentLogEntry
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int OldRate { get; set; }

        public int NewRate { get; set; }
    }
}