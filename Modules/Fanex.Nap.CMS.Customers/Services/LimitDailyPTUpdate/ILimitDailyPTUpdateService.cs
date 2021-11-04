namespace Fanex.Nap.CMS.Customers.Services.LimitDailyPTUpdate
{
    using Fanex.Nap.CMS.Customers.Models.LimitDailyPTUpdate;
    using Fanex.Nap.CMS.Customers.ViewModels.LimitDailyPTUpdate;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface ILimitDailyPTUpdateService
    {
        IEnumerable<SelectListItem> GetSites();

        IEnumerable<SelectListItem> GetProducts();

        IEnumerable<SelectListItem> GetCurrencies();

        LimitDailyPTUpdateReport GetLimitReport(SearchDailyPTUpdateViewModel viewModel);

        AddNewOrRemoveLimitResult AddNewLimit(AddNewLimitDailyPTUpdateViewModel viewModel);

        AddNewOrRemoveLimitResult RemoveLimit(RemoveLimitDailyPTUpdateViewModel viewModel);
    }
}
