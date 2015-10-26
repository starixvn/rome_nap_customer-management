namespace Fanex.Nap.CMS.WinLimitControl.Repositories.CurrencySetting
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.WinLimitControl.Models.CurrencySetting;

    public interface ICurrencySettingRepository
    {
        IList<WinLimitEntity> GetCurrencySettings(ICriteria criteria);

        Result AddCurrencySetting(ICriteria criteria);

        Result UpdateCurrencySetting(ICriteria criteria);

        IList<GreaterPlayerWinLimit> GetGreaterPlayerWinLimits(ICriteria criteria);
    }
}
