using System.Collections.Generic;
using System.Web.Mvc;
using Fanex.Nap.CMS.Infrastructure.Constants;

namespace Fanex.Nap.CMS.Customers.ViewModels.LimitNewDownline
{
    public class LimitNewDownlineViewModel
    {
        public LimitNewDownlineViewModel(IEnumerable<string> sites, bool hasHostPermission)
        {
            Sites = sites;
            HasHostPermission = hasHostPermission;
        }

        public IEnumerable<string> Sites { get; }

        public bool HasHostPermission { get; }

        public IEnumerable<SelectListItem> Levels()
            => new[]
            {
                BuildSelectListItem(nameof(RoleIdConstants.All), RoleIdConstants.All, true),
                BuildSelectListItem(nameof(RoleIdConstants.Super), RoleIdConstants.Super),
                BuildSelectListItem(nameof(RoleIdConstants.Master), RoleIdConstants.Master),
                BuildSelectListItem(nameof(RoleIdConstants.Agent), RoleIdConstants.Agent)
            };

        private static SelectListItem BuildSelectListItem(string text, int value, bool selected = false)
            => new SelectListItem { Text = text, Value = value.ToString(), Selected = selected };
    }
}
