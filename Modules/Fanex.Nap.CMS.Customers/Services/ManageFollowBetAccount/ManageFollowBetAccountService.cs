namespace Fanex.Nap.CMS.Customers.Services.ManageFollowBetAccount
{
    using Fanex.Nap.CMS.Customers.Criteria.ManageFollowBetAccount;
    using Fanex.Nap.CMS.Customers.Models.ManageFollowBetAccount;
    using Fanex.Nap.CMS.Customers.Repositories.ManageFollowBetAccount;
    using Fanex.Nap.CMS.Customers.ViewModels.ManageFollowBetAccount;
    using Fanex.Nap.CMS.Infrastructure.Configurations;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class ManageFollowBetAccountService : IManageFollowBetAccountService
    {
        private readonly IManageFollowBetAccountRepository repository;

        public ManageFollowBetAccountService(IManageFollowBetAccountRepository repository = null)
        {
            this.repository = repository ?? new ManageFollowBetAccountRepository();
        }

        public IEnumerable<SelectListItem> GetSites()
            => repository.GetSites(new GetCreditSiteCriteria(0));

        public IEnumerable<SelectListItem> GetCurrencies()
           => repository.GetCurrencies(new GetCurrencyListCriteria());

        public IEnumerable<SelectListItem> GetDummyAgents()
            => repository.GetDummyAgents(new GetDummyAgentCriteria());

        public IEnumerable<FollowBetAccountItem> GetFollowBetAccountList(SearchFollowBetAccountViewModel viewModel)
            => repository.GetFollowBetAccountList(new GetFollowBetAccountListCriteria(
                viewModel.Username ?? string.Empty,
                viewModel.CurrencyId,
                viewModel.SiteName ?? string.Empty,
                viewModel.DummyAgentId,
                viewModel.IsActive
                ));

        public Result AddNewFollowBetAccount(AddNewFollowBetAccountViewModel viewModel)
        {
            var result = repository.ExecuteNonQuery(
                        new AddNewFollowBetAccountCriteria(
                            viewModel.DummyAgentId,
                            viewModel.CustList,
                            viewModel.UserId,
                            viewModel.IP
                        ));

            if (result.IsSuccess)
            {
                result.Message = Resources.Labels.AllAccountHaveBeenSuccessfullyAdded;
            }

            return result;
        }

        public Result UpdateFollowBetAccount(UpdateFollowBetAccountViewModel viewModel)
        {
            var result = repository.ExecuteNonQuery(
                        new UpdateFollowBetAccountCriteria(
                            viewModel.Username,
                            viewModel.DummyAgentId,
                            viewModel.IsActive,
                            viewModel.UserId,
                            viewModel.IP
                        ));

            if (result.IsSuccess)
            {
                result.Message = Resources.Labels.UpdatedSuccessfully;
            }

            return result;
        }

        public Result RemoveFollowBetAccount(RemoveFollowBetAccountViewModel viewModel)
        {
            var result = repository.ExecuteNonQuery(
                        new RemoveFollowBetAccountCriteria(
                            viewModel.Username,
                            viewModel.DummyAgentId,
                            viewModel.UserId,
                            viewModel.IP
                        ));

            if (result.IsSuccess)
            {
                result.Message = Resources.Labels.RemovedSuccessfully;
            }

            return result;
        }
    }
}
