using Fanex.Nap.CMS.Infrastructure.Models;

namespace Fanex.Nap.CMS.Customers.Services.LimitDailyPTUpdate
{
    using Fanex.Nap.CMS.Customers.Criteria.LimitDailyPTUpdate;
    using Fanex.Nap.CMS.Customers.Criteria.LimitNewDownline;
    using Fanex.Nap.CMS.Customers.Models.LimitDailyPTUpdate;
    using Fanex.Nap.CMS.Customers.Resources;
    using Fanex.Nap.CMS.Customers.ViewModels.LimitDailyPTUpdate;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class LimitDailyPTUpdateService : ServiceBase, ILimitDailyPTUpdateService
    {
        public IEnumerable<SelectListItem> GetSites()
        {
            return DynamicRepository.Fetch<IdNameModel>(new GetCreditSiteCriteria(0))
                .Select(site => new SelectListItem { Text = site.Name, Value = site.Id.ToString() });
        }

        public IEnumerable<SelectListItem> GetProducts()
        {
            return DynamicRepository.Fetch<ProductDTO>(new GetProductListCriteria())
                .Select(product => new SelectListItem { Text = product.ProductName, Value = product.ProductId.ToString() });
        }

        public IEnumerable<SelectListItem> GetCurrencies()
        {
            return DynamicRepository.Fetch<CurrencyModel>(new GetCurrencyListCriteria())
                .Select(currency => new SelectListItem { Text = currency.Currency, Value = currency.ExchangeId.ToString() });
        }

        public LimitDailyPTUpdateReport GetLimitReport(SearchDailyPTUpdateViewModel viewModel)
        {
            var criteria = new GetLimitDailyPTUpdateListCriteria(
                    viewModel.Username == null ? string.Empty : viewModel.Username.Trim(),
                    viewModel.SiteNames ?? string.Empty,
                    viewModel.CurrencyIds ?? string.Empty,
                    viewModel.ProductId ?? string.Empty,
                    viewModel.TypeId,
                    viewModel.PageIndex,
                    viewModel.PageSize, 
                    viewModel.LimitUpdatePT);
            var limitDailyPTUpdateList = DynamicRepository.Fetch<LimitDailyPTUpdateItem>(criteria);

            return new LimitDailyPTUpdateReport(limitDailyPTUpdateList, criteria.TotalItems);
        }

        public Result AddNewLimit(AddNewLimitDailyPTUpdateViewModel viewModel)
        {
            var criteria = new AddNewLimitDailyPTUpdateCriteria(
                    viewModel.Product,
                    viewModel.SMAs,
                    viewModel.Currency,
                    viewModel.Site,
                    viewModel.Type,
                    viewModel.UserId,
                    viewModel.IP,
                    viewModel.LimitNewAccUpdPT
                );

            return DoAction(criteria, LimitDailyPTUpdateResource.AddLimitSuccessfully);
        }

        public Result RemoveLimit(RemoveLimitDailyPTUpdateViewModel viewModel)
        {
            var criteria = new RemoveLimitDailyPTUpdateCriteria(
                   viewModel.ProductID,
                   viewModel.CustID,
                   viewModel.SiteID,
                   viewModel.Currency,
                   viewModel.Type,
                   viewModel.UserId,
                   viewModel.IP
               );

            return DoAction(criteria, LimitDailyPTUpdateResource.RemoveLimitSuccessfully);
        }

        public Result UpdateLimit(UpdateLimitPTUpdateNewAccountViewModel viewModel)
        {
            var criteria = new UpdateLimitPTUpdateNewAccountCriteria(
                    viewModel.ProductID,
                    viewModel.CustID,
                    viewModel.SiteID,
                    viewModel.Currency,
                    viewModel.Type,
                    viewModel.UserId,
                    viewModel.IP,
                    viewModel.LimitNewAccUpdPT
                );

            return DoAction(criteria, LimitDailyPTUpdateResource.UpdateLimitSuccessfully);
        }
    }
}
