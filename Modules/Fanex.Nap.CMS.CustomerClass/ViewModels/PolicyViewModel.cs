using Fanex.Nap.CMS.Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Fanex.Nap.CMS.CustomerClass.ViewModels
{
    public class PolicyViewModel : BaseViewModel
    {
        public PolicyViewModel(IEnumerable<IdNameModel> sportTypes, bool hasHostPermission) : base(hasHostPermission)
        {
            SportTypes = sportTypes.Select(c => ToSelectListItem(c));
        }

        public IEnumerable<SelectListItem> SportTypes { get; }
    }
}
