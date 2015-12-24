namespace Fanex.Nap.CMS.Settings.Services
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Settings.Models;

    public interface ISettingService
    {
        IList<BaseSetting> GetCurrencies();

        IList<BaseSetting> GetGames();
    }
}
