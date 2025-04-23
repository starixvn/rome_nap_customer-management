using Fanex.Nap.CMS.ManageProductSetting.Models.CompanyBetLimitSetting;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fanex.Nap.CMS.ManageProductSetting.ViewModels.CompanyBetLimitSetting
{
    public class GetCurrencyGroupViewModel
    {
        public bool IsCredit { get; set; }
        public IEnumerable<SelectListItem> Currencies { get; set; }
        public IEnumerable<CurrencyGroup> CurrencyGroups { get; set; }
    }
}