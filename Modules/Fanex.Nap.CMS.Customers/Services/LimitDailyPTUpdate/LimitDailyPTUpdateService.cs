using Fanex.Nap.CMS.Infrastructure.Models;

namespace Fanex.Nap.CMS.Customers.Services.LimitDailyPTUpdate
{
    using Fanex.Nap.CMS.Customers.Criteria.LimitDailyPTUpdate;
    using Fanex.Nap.CMS.Customers.Criteria.LimitNewDownline;
    using Fanex.Nap.CMS.Customers.Models.LimitDailyPTUpdate;
    using Fanex.Nap.CMS.Customers.Repositories.LimitDailyPTUpdate;
    using Fanex.Nap.CMS.Customers.Resources;
    using Fanex.Nap.CMS.Customers.ViewModels.LimitDailyPTUpdate;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class LimitDailyPTUpdateService : ILimitDailyPTUpdateService
    {
        private readonly ILimitDailyPTUpdateRepository repository;

        public LimitDailyPTUpdateService(ILimitDailyPTUpdateRepository repository = null)
        {
            this.repository = repository ?? new LimitDailyPTUpdateRepository();
        }

        public IEnumerable<SelectListItem> GetSites()
            => repository.GetSites(new GetCreditSiteCriteria(0));

        public IEnumerable<SelectListItem> GetProducts()
           => repository.GetProducts(new GetProductListCriteria(0));

        public IEnumerable<SelectListItem> GetCurrencies()
           => repository.GetCurrencies(new GetCurrencyListCriteria());

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
            var limitDailyPTUpdateList = repository.GetLimitDailyPTUpdateList(criteria);

            return new LimitDailyPTUpdateReport(limitDailyPTUpdateList, criteria.TotalItems);
        }

        public Result AddNewLimit(AddNewLimitDailyPTUpdateViewModel viewModel)
        {
            var result = repository.ExecuteNonQuery(
                new AddNewLimitDailyPTUpdateCriteria(
                    viewModel.Product,
                    viewModel.SMAs,
                    viewModel.Currency,
                    viewModel.Site,
                    viewModel.Type,
                    viewModel.UserId,
                    viewModel.IP,
                    viewModel.LimitNewAccUpdPT
                ));

            if (result.IsSuccess)
            {
                result.Message = LimitDailyPTUpdateResource.AddLimitSuccessfully;
            }

            return result;
        }

        public Result RemoveLimit(RemoveLimitDailyPTUpdateViewModel viewModel)
        {
            var result = repository.ExecuteNonQuery(
               new RemoveLimitDailyPTUpdateCriteria(
                   viewModel.ProductID,
                   viewModel.CustID,
                   viewModel.SiteID,
                   viewModel.Currency,
                   viewModel.Type,
                   viewModel.UserId,
                   viewModel.IP
               ));

            if (result.IsSuccess)
            {
                result.Message = LimitDailyPTUpdateResource.RemoveLimitSuccessfully;
            }

            return result;
        }

        public Result UpdateLimit(UpdateLimitPTUpdateNewAccountViewModel viewModel)
        {
            var result = repository.ExecuteNonQuery(
                           new UpdateLimitPTUpdateNewAccountCriteria(
                               viewModel.ProductID,
                               viewModel.CustID,
                               viewModel.SiteID,
                               viewModel.Currency,
                               viewModel.Type,
                               viewModel.UserId,
                               viewModel.IP,
                               viewModel.LimitNewAccUpdPT
                           ));

            if (result.IsSuccess)
            {
                result.Message = LimitDailyPTUpdateResource.UpdateLimitSuccessfully;
            }

            return result;
        }
    }
}
