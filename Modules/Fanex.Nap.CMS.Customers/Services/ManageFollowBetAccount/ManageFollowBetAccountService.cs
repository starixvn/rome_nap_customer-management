namespace Fanex.Nap.CMS.Customers.Services.ManageFollowBetAccount
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Customers.Criteria.ManageFollowBetAccount;
    using Fanex.Nap.CMS.Customers.Models.ManageFollowBetAccount;
    using Fanex.Nap.CMS.Customers.ViewModels.ManageFollowBetAccount;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;

    public class ManageFollowBetAccountService : ServiceBase, IManageFollowBetAccountService
    {
        public IEnumerable<SelectListItem> GetSites()
        {
            return DynamicRepository.Fetch<IdNameModel>(new GetCreditSiteCriteria(0))
                .Select(site => new SelectListItem { Text = site.Name, Value = site.Id.ToString() });
        }

        public IEnumerable<SelectListItem> GetCurrencies()
        {
            return DynamicRepository.Fetch<CurrencyModel>(new GetCurrencyListCriteria())
                .Select(currency => new SelectListItem { Text = currency.Currency, Value = currency.ExchangeId.ToString() });
        }

        public IEnumerable<SelectListItem> GetDummyAgents()
        {
            return DynamicRepository.Fetch<DummyAgentDTO>(new GetDummyAgentCriteria())
                .Select(agent => new SelectListItem { Text = agent.Username, Value = agent.CustId.ToString() });
        }

        public IEnumerable<FollowBetAccountItem> GetFollowBetAccountList(SearchFollowBetAccountViewModel viewModel)
            => DynamicRepository.Fetch<FollowBetAccountItem>(new GetFollowBetAccountListCriteria(
                viewModel.Username ?? string.Empty,
                viewModel.CurrencyId,
                viewModel.SiteName ?? string.Empty,
                viewModel.DummyAgentId,
                viewModel.IsActive,
                viewModel.IsFollow));

        public Result AddNewFollowBetAccount(AddNewFollowBetAccountViewModel viewModel)
        {
            var criteria = new AddNewFollowBetAccountCriteria(
                    viewModel.DummyAgentId,
                    viewModel.CustList,
                    viewModel.UserId,
                    viewModel.IP);

            return DoAction(criteria, Resources.Labels.AllAccountHaveBeenSuccessfullyAdded);
        }

        public Result UpdateFollowBetAccount(UpdateFollowBetAccountViewModel viewModel)
        {
            var criteria = new UpdateFollowBetAccountCriteria(
                    viewModel.Username,
                    viewModel.DummyAgentId,
                    viewModel.IsActive,
                    viewModel.UserId,
                    viewModel.IP);

            return DoAction(criteria, Resources.Labels.UpdatedSuccessfully);
        }

        public Result RemoveFollowBetAccount(RemoveFollowBetAccountViewModel viewModel)
        {
            var criteria = new RemoveFollowBetAccountCriteria(
                    viewModel.Username,
                    viewModel.DummyAgentId,
                    viewModel.UserId,
                    viewModel.IP);

            return DoAction(criteria, Resources.Labels.RemovedSuccessfully);
        }

        public Result RemoveFollowBetAccounts(RemoveFollowBetAccountsViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Usernames))
            {
                return new Result(0, string.Empty);

            }

            var usernames = viewModel.Usernames.Split(new[] { ',' });

            foreach (var username in usernames)
            {
                var result = RemoveFollowBetAccount(new RemoveFollowBetAccountViewModel
                {
                    Username = username,
                    DummyAgentId = viewModel.DummyAgentId,
                    UserId = viewModel.UserId,
                    IP = viewModel.IP,
                });

                if (!result.IsSuccess)
                {
                    return result;
                }
            }

            return new Result(0, Resources.Labels.RemovedSuccessfully);
        }

        public Result UpdateFollowStatus(UpdateFollowBetAccountViewModel viewModel)
        {
            var criteria = new UpdateFollowStatusCriteria(
                viewModel.Username,
                viewModel.DummyAgentId,
                viewModel.IsActive,
                viewModel.UserId,
                viewModel.IP);

            return DoAction(criteria);
        }
    }
}
