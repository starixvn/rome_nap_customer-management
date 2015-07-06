namespace Fanex.Nap.CMS.WinLimitControl.Models.CurrencySetting
{
    using System.Collections.Generic;

    public class CurrencyWinLimit
    {
        public string Currency { get; set; }

        public IList<WinLimitEntity> WinLimits { get; set; }
    }
}
