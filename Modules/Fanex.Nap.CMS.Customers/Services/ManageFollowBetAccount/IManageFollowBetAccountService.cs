namespace Fanex.Nap.CMS.Customers.Services.ManageFollowBetAccount
{
    using Fanex.Nap.CMS.Customers.Models.ManageFollowBetAccount;
    using Fanex.Nap.CMS.Customers.ViewModels.ManageFollowBetAccount;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public interface IManageFollowBetAccountService
    {
        IEnumerable<SelectListItem> GetSites();

        IEnumerable<SelectListItem> GetCurrencies();

        IEnumerable<SelectListItem> GetDummyAgents();

        IEnumerable<FollowBetAccountItem> GetFollowBetAccountList(SearchFollowBetAccountViewModel viewModel);

        Result RemoveFollowBetAccount(RemoveFollowBetAccountViewModel viewModel);

        Result AddNewFollowBetAccount(AddNewFollowBetAccountViewModel viewModel);

        Result UpdateFollowBetAccount(UpdateFollowBetAccountViewModel viewModel);

        Result UpdateFollowStatus(UpdateFollowBetAccountViewModel viewModel);

        Result RemoveFollowBetAccounts(RemoveFollowBetAccountsViewModel viewModel);
    }
}
