namespace Fanex.Nap.CMS.Customers.ViewModels.ManageFollowBetAccount
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class ManageFollowBetAccountViewModel
    {
        public ManageFollowBetAccountViewModel(
            bool hasHostPermission, 
            IEnumerable<SelectListItem> sites, 
            IEnumerable<SelectListItem> currencies,
            IEnumerable<SelectListItem> dummyAgents)
        {
            HasHostPermission = hasHostPermission;
            Sites = sites;
            Currencies = currencies;
            DummyAgents = dummyAgents;
        }

        public bool HasHostPermission { get; }

        public IEnumerable<SelectListItem> Sites { get; }

        public IEnumerable<SelectListItem> Currencies { get; }

        public IEnumerable<SelectListItem> DummyAgents { get; }
    }
}
