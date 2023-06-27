using Fanex.Nap.CMS.Infrastructure.Models;

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

        Result AddNewLimit(AddNewLimitDailyPTUpdateViewModel viewModel);

        Result RemoveLimit(RemoveLimitDailyPTUpdateViewModel viewModel);

        Result UpdateLimit(UpdateLimitPTUpdateNewAccountViewModel viewModel);
    }
}
