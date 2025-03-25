namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails
{
    using System;

    public class LogDTO : VirtualRateDTO
    {
        public string LogId { get; set; }

        public string Username { get; set; }

        public decimal ExchangeRate { get; set; } = -1;

        public string ExchangeRateText { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string Editor { get; set; }

        public bool IsNew { get; set; }
    }
}
