namespace Fanex.Nap.CMS.Customers.Services.LimitDailyPTUpdate
{
    using Fanex.Nap.CMS.Customers.Models.LimitDailyPTUpdate;
    using Fanex.Nap.CMS.Customers.ViewModels.LimitDailyPTUpdate;
    using System.Collections.Generic;

    public interface ILimitDailyPTUpdateService
    {
        IEnumerable<string> GetSites();

        IEnumerable<string> GetProducts();

        IEnumerable<string> GetCurrencies();

        LimitDailyPTUpdateReport GetLimitReport(SearchDailyPTUpdateViewModel viewModel);

        AddNewOrRemoveLimitResult AddNewLimit(AddNewLimitDailyPTUpdateViewModel viewModel);

        AddNewOrRemoveLimitResult RemoveLimit(string username);
    }
}
