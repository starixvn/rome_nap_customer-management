namespace Fanex.Nap.CMS.Customers.Services.LimitDailyPTUpdate
{
    using Fanex.Nap.CMS.Customers.Criteria.LimitDailyPTUpdate;
    using Fanex.Nap.CMS.Customers.Models.LimitDailyPTUpdate;
    using Fanex.Nap.CMS.Customers.Repositories.LimitDailyPTUpdate;
    using Fanex.Nap.CMS.Customers.ViewModels.LimitDailyPTUpdate;
    using System.Collections.Generic;

    public class LimitDailyPTUpdateService : ILimitDailyPTUpdateService
    {
        private readonly ILimitDailyPTUpdateRepository repository;

        public LimitDailyPTUpdateService(ILimitDailyPTUpdateRepository repository = null)
        {
            this.repository = repository ?? new LimitDailyPTUpdateRepository();
        }

        public IEnumerable<string> GetSites()
            => repository.GetSites();

        public IEnumerable<string> GetProducts()
           => repository.GetProducts();

        public IEnumerable<string> GetCurrencies()
           => repository.GetCurrencies();

        public LimitDailyPTUpdateReport GetLimitReport(SearchDailyPTUpdateViewModel viewModel)
        {
            var criteria = new GetLimitDailyPTUpdateListCriteria(
                    viewModel.Username == null ? string.Empty : viewModel.Username.Trim(),
                    viewModel.Site,
                    viewModel.Currency,
                    viewModel.Product,
                    viewModel.Type,
                    viewModel.PageIndex,
                    viewModel.PageSize);
            var limitDailyPTUpdateList = repository.GetLimitDailyPTUpdateList(criteria);

            // TODO: Update criteria.TotalItems as 2nd params
            return new LimitDailyPTUpdateReport(limitDailyPTUpdateList, 3);
        }

        public AddNewOrRemoveLimitResult AddNewLimit(AddNewLimitDailyPTUpdateViewModel viewModel)
        {
            return new AddNewOrRemoveLimitResult(0, "Successfully added");
        }

        public AddNewOrRemoveLimitResult RemoveLimit(string username)
        {
            return new AddNewOrRemoveLimitResult(0, "Successfully removed " + username);
        }
    }
}
