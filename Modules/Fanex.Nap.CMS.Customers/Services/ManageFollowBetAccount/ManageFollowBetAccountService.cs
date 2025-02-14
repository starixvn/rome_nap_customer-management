namespace Fanex.Nap.CMS.Customers.Services.ManageFollowBetAccount
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Customers.Criteria.ManageFollowBetAccount;
    using Fanex.Nap.CMS.Customers.Models.ManageFollowBetAccount;
    using Fanex.Nap.CMS.Customers.Repositories.ManageFollowBetAccount;
    using Fanex.Nap.CMS.Customers.ViewModels.ManageFollowBetAccount;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Resources;

    public class ManageFollowBetAccountService : IManageFollowBetAccountService
    {
        private readonly IManageFollowBetAccountRepository repository;

        public ManageFollowBetAccountService(IManageFollowBetAccountRepository repository = null)
            => this.repository = repository ?? new ManageFollowBetAccountRepository();

        public IEnumerable<SelectListItem> GetSites()
            => repository.GetSites(new GetCreditSiteCriteria(0));

        public IEnumerable<SelectListItem> GetCurrencies()
           => repository.GetCurrencies(new GetCurrencyListCriteria());

        public IEnumerable<SelectListItem> GetDummyAgents()
            => repository.GetDummyAgents(new GetDummyAgentCriteria()).ToList();

        public IEnumerable<FollowBetAccountItem> GetFollowBetAccountList(SearchFollowBetAccountViewModel viewModel)
            => repository.GetFollowBetAccountList(new GetFollowBetAccountListCriteria(
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

            if (!criteria.IsValid())
            {
                return new Result((int)HttpStatusCode.Forbidden, Labels.AccessDenied);
            }

            repository.ExecuteNonQuery(criteria);
            var result = new Result(
                criteria.ErrCode,
                criteria.ErrMsg);

            if (result.IsSuccess)
            {
                result.Message = Resources.Labels.AllAccountHaveBeenSuccessfullyAdded;
            }

            return result;
        }

        public Result UpdateFollowBetAccount(UpdateFollowBetAccountViewModel viewModel)
        {
            var criteria = new UpdateFollowBetAccountCriteria(
                    viewModel.Username,
                    viewModel.DummyAgentId,
                    viewModel.IsActive,
                    viewModel.UserId,
                    viewModel.IP);

            if (!criteria.IsValid())
            {
                return new Result((int)HttpStatusCode.Forbidden, Labels.AccessDenied);
            }

            repository.ExecuteNonQuery(criteria);
            var result = new Result(
                criteria.ErrCode,
                criteria.ErrMsg);

            if (result.IsSuccess)
            {
                result.Message = Resources.Labels.UpdatedSuccessfully;
            }

            return result;
        }

        public Result RemoveFollowBetAccount(RemoveFollowBetAccountViewModel viewModel)
        {
            var criteria = new RemoveFollowBetAccountCriteria(
                    viewModel.Username,
                    viewModel.DummyAgentId,
                    viewModel.UserId,
                    viewModel.IP);

            if (!criteria.IsValid())
            {
                return new Result((int)HttpStatusCode.Forbidden, Labels.AccessDenied);
            }

            repository.ExecuteNonQuery(criteria);
            var result = new Result(
                criteria.ErrCode,
                criteria.ErrMsg);

            if (result.IsSuccess)
            {
                result.Message = Resources.Labels.RemovedSuccessfully;
            }

            return result;
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

            if (!criteria.IsValid())
            {
                return new Result((int)HttpStatusCode.Forbidden, Labels.AccessDenied);
            }

            repository.ExecuteNonQuery(criteria);

            return new Result(
                criteria.ErrCode,
                criteria.ErrMsg);
        }
    }
}
