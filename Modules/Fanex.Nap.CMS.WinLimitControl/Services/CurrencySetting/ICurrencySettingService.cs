namespace Fanex.Nap.CMS.WinLimitControl.Services.CurrencySetting
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.CurrencySetting;
    using Fanex.Nap.CMS.WinLimitControl.Models.CurrencySetting;

    public interface ICurrencySettingService
    {
        IList<WinLimitEntity> GetCurrencySettings(GetCurrencyWinLimitCriteria criteria);

        Result AddCurrencySetting(AddCurrencyWinLimitCriteria criteria);

        Result UpdateCurrencySetting(UpdateCurrencyWinLimitCriteria criteria);

        IList<GreaterPlayerWinLimit> GetGreaterPlayerWinLimits(ICriteria criteria);
    }
}
